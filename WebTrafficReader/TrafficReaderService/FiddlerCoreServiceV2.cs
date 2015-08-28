using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficReaderService
{
    /// <summary>
    /// https://github.com/RickStrahl/WestWindWebSurge/blob/master/WebSurge/FiddlerCapture.cs
    /// </summary>
    public class FiddlerCoreServiceV2
    {
        public void Start()
        {
            //if (tbIgnoreResources.Checked)
            //    CaptureConfiguration.IgnoreResources = true;
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
            //CaptureConfiguration.CaptureDomain = txtCaptureDomain.Text;

            //FiddlerApplication.BeforeRequest        += FiddlerApplication_BeforeRequest;
            //FiddlerApplication.BeforeResponse       += FiddlerApplication_BeforeResponse;
            FiddlerApplication.AfterSessionComplete += FiddlerApplication_AfterSessionComplete;
            FiddlerApplication.Startup(8888, true, true, true);
        }

        private void FiddlerApplication_AfterSessionComplete(Session sess)
        {
            // Ignore HTTPS connect requests
            if (sess.RequestMethod == "CONNECT")
                return;

            //if (CaptureConfiguration.ProcessId > 0)
            //{
            //    if (sess.LocalProcessID != 0 && sess.LocalProcessID != CaptureConfiguration.ProcessId)
            //        return;
            //}

            //if (!string.IsNullOrEmpty(CaptureConfiguration.CaptureDomain))
            //{
            //    if (sess.hostname.ToLower() != CaptureConfiguration.CaptureDomain.Trim().ToLower())
            //        return;
            //}

            //if (sess.hostname.ToLower() != "bet365" && sess.hostname.ToLower() != "www.bet365.com")
            //{
            //    return;
            //}

            //if (CaptureConfiguration.IgnoreResources)
            //{
            //    string url = sess.fullUrl.ToLower();

            //    var extensions = CaptureConfiguration.ExtensionFilterExclusions;
            //    foreach (var ext in extensions)
            //    {
            //        if (url.Contains(ext))
            //            return;
            //    }

            //    var filters = CaptureConfiguration.UrlFilterExclusions;
            //    foreach (var urlFilter in filters)
            //    {
            //        if (url.Contains(urlFilter))
            //            return;
            //    }
            //}

            if (sess == null || sess.oRequest == null || sess.oRequest.headers == null)
                return;

            string headers = sess.oRequest.headers.ToString();
            var reqBody    = sess.GetRequestBodyAsString();

            // if you wanted to capture the response
            string respHeaders = sess.oResponse.headers.ToString();
            var respBody       = sess.GetResponseBodyAsString();

            // replace the HTTP line to inject full URL
            //string firstLine = sess.RequestMethod + " " + sess.fullUrl + " " + sess.oRequest.headers.HTTPVersion;
            //int at = headers.IndexOf("\r\n");
            //if (at < 0)
            //    return;
            //headers = firstLine + "\r\n" + headers.Substring(at + 1);

            //string output = headers + "\r\n" +
            //                (!string.IsNullOrEmpty(reqBody) ? reqBody + "\r\n" : string.Empty) +
            //                Separator + "\r\n\r\n";

            var output = string.Format("Headers: {0}; /n Body: {1}; /n RHeaders: {2}; /n RBody: {3}",headers, reqBody, respHeaders, respBody);

            //BeginInvoke(new Action<string>((text) =>
            //{
            //    txtCapture.AppendText(text);
            //    UpdateButtonStatus();
            //}), output);

            Console.WriteLine(output);
        }
    }
}
