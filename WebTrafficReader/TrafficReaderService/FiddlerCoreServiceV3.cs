using Fiddler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TrafficReaderService
{
    /// <summary>
    /// http://weblog.west-wind.com/posts/2014/Jul/29/Using-FiddlerCore-to-capture-HTTP-Requests-with-NET#IntegratingFiddlerCore
    /// http://www.telerik.com/blogs/faq---certificates-in-fiddler
    /// </summary>
    public class FiddlerCoreServiceV3
    {
        private const string Separator = "------------------------------------------------------------------";
        private UrlCaptureConfiguration CaptureConfiguration { get; set; }

        public FiddlerCoreServiceV3()
        {
            CaptureConfiguration = new UrlCaptureConfiguration();
        }

        public void Start()
        {
            //if (tbIgnoreResources.Checked)
                CaptureConfiguration.IgnoreResources = true;
            //else
            //    CaptureConfiguration.IgnoreResources = false;

            //string strProcId = txtProcessId.Text;
            //if (strProcId.Contains('-'))
            //    strProcId = strProcId.Substring(strProcId.IndexOf('-') + 1).Trim();

            //strProcId = strProcId.Trim();

            //int procId = 0;
            //if (!string.IsNullOrEmpty(strProcId))
            //{
            //    if (!int.TryParse(strProcId, out procId))
            //        procId = 0;
            //}
            //CaptureConfiguration.ProcessId = procId;
            CaptureConfiguration.CaptureDomain = "www.abola.pt";

            FiddlerApplication.AfterSessionComplete += FiddlerApplication_AfterSessionComplete;
            //FiddlerApplication.Startup(8888, true, true, true);

            const FiddlerCoreStartupFlags flags =
                    FiddlerCoreStartupFlags.AllowRemoteClients |
                    FiddlerCoreStartupFlags.CaptureLocalhostTraffic |
                    FiddlerCoreStartupFlags.DecryptSSL |
                    FiddlerCoreStartupFlags.MonitorAllConnections |
                    FiddlerCoreStartupFlags.RegisterAsSystemProxy;

            FiddlerApplication.Startup(CaptureConfiguration.ProxyPort, flags);
        }

        void Stop()
        {
            FiddlerApplication.AfterSessionComplete -= FiddlerApplication_AfterSessionComplete;

            if (FiddlerApplication.IsStarted())
                FiddlerApplication.Shutdown();
        }

        private void FiddlerApplication_AfterSessionComplete(Session sess)
        {
            // Ignore HTTPS connect requests

            if (sess.RequestMethod == "CONNECT")
                return;

            if (CaptureConfiguration.ProcessId > 0)
            {
                if (sess.LocalProcessID != 0 && sess.LocalProcessID != CaptureConfiguration.ProcessId)
                    return;
            }

            if (!string.IsNullOrEmpty(CaptureConfiguration.CaptureDomain))
            {
                if (sess.hostname.ToLower() != CaptureConfiguration.CaptureDomain.Trim().ToLower())
                    return;
            }

            if (CaptureConfiguration.IgnoreResources)
            {
                string url = sess.fullUrl.ToLower();

                var extensions = CaptureConfiguration.ExtensionFilterExclusions;
                foreach (var ext in extensions)
                {
                    if (url.Contains(ext))
                        return;
                }

                var filters = CaptureConfiguration.UrlFilterExclusions;
                foreach (var urlFilter in filters)
                {
                    if (url.Contains(urlFilter))
                        return;
                }
            }

            if (sess == null || sess.oRequest == null || sess.oRequest.headers == null)
                return;

            string headers = sess.oRequest.headers.ToString();

            string contentType =
                sess.oRequest.headers.Where(hd => hd.Name.ToLower() == "content-type")
                    .Select(hd => hd.Name)
                    .FirstOrDefault();

            string reqBody = null;
            if (sess.RequestBody.Length > 0)
            {

                if (sess.requestBodyBytes.Contains((byte)0) || contentType.StartsWith("image/"))
                    reqBody = "b64_" + Convert.ToBase64String(sess.requestBodyBytes);
                else
                {
                    //reqBody = Encoding.Default.GetString(sess.ResponseBody);
                    reqBody = sess.GetRequestBodyAsString();
                }
            }

            // if you wanted to capture the response
            //string respHeaders = session.oResponse.headers.ToString();
            //var respBody = Encoding.UTF8.GetString(session.ResponseBody);

            // replace the HTTP line to inject full URL
            string firstLine = sess.RequestMethod + " " + sess.fullUrl + " " + sess.oRequest.headers.HTTPVersion;
            int at = headers.IndexOf("\r\n");
            if (at < 0)
                return;
            headers = firstLine + "\r\n" + headers.Substring(at + 1);

            string output = headers + "\r\n" +
                            (!string.IsNullOrEmpty(reqBody) ? reqBody + "\r\n" : string.Empty) +
                            Separator + "\r\n\r\n";

            // must marshal and synchronize to UI thread
            //BeginInvoke(new Action<string>((text) =>
            //{
            //    try
            //    {
            //        txtCapture.AppendText(text);
            //    }
            //    catch (Exception e)
            //    {
            //        App.Log(e);
            //    }

            //    UpdateButtonStatus();
            //}), output);
        }

        public static bool InstallCertificate()
        {
            if (!CertMaker.rootCertExists())
            {
                if (!CertMaker.createRootCert())
                    return false;

                if (!CertMaker.trustRootCert())
                    return false;
            }

            return true;
        }

        public static bool UninstallCertificate()
        {
            if (CertMaker.rootCertExists())
            {
                if (!CertMaker.removeFiddlerGeneratedCerts(true))
                    return false;
            }
            return true;
        }
    }  

    public class UrlCaptureConfiguration
    {

        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public int ProcessId { get; set; }

        public int ProxyPort { get; set; }
        public bool IgnoreResources { get; set; }
        public string CaptureDomain { get; set; }
        public List<string> UrlFilterExclusions { get; set; }
        public List<string> ExtensionFilterExclusions { get; set; }

        [Browsable(false)]
        public string Cert { get; set; }

        [Browsable(false)]
        public string Key { get; set; }


        public UrlCaptureConfiguration()
        {
            ProxyPort = 8888;
            UrlFilterExclusions = new List<string>();
            ExtensionFilterExclusions = new List<string>();
        }
    }
}
