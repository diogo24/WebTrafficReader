using Fiddler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrafficReaderService
{
    public class FiddlerCoreService : IDisposable
    {
        //https://msdn.microsoft.com/en-us/library/8bh11f1k.aspx
        private StreamWriter logFileStreamWriter;
        private string logFilePath = @"C:\Users\diogo.marques\Documents\GitHubVisualStudio\WebTrafficReader\FiddlerLogFiles\Fiddler_Log_and_Notifications.txt";

        public FiddlerCoreService()
        {
            logFileStreamWriter = new StreamWriter(logFilePath, true);
        }

        #region IDisposable

        // Flag: Has Dispose already been called? 
        bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                logFileStreamWriter.Flush();
                logFileStreamWriter.Close();
                logFileStreamWriter.Dispose();

                //specificLogFileStreamWriter.Flush();
                //specificLogFileStreamWriter.Close();
                //specificLogFileStreamWriter.Dispose();
                // Free any other managed objects here. 
                //
            }

            // Free any unmanaged objects here. 
            //
            disposed = true;
        }

        #endregion

        public void Start()
        {
            List<Fiddler.Session> allFidlerSessions = new List<Session>();

            // <-- Personalize for your Application, 64 chars or fewer
            Fiddler.FiddlerApplication.SetAppDisplayName("FiddlerCoreTrafficReaderService");

            #region AttachEventListeners
            //
            // It is important to understand that FiddlerCore calls event handlers on session-handling
            // background threads.  If you need to properly synchronize to the UI-thread (say, because
            // you're adding the sessions to a list view) you must call .Invoke on a delegate on the 
            // window handle.
            // 
            // If you are writing to a non-threadsafe data structure (e.g. List<t>) you must
            // use a Monitor or other mechanism to ensure safety.
            //

            // Simply echo notifications to the console.  Because Fiddler.CONFIG.QuietMode=true 
            // by default, we must handle notifying the user ourselves.
            Fiddler.FiddlerApplication.OnNotification += delegate (object sender, NotificationEventArgs oNEA) {
                logFileStreamWriter.WriteLine("** NotifyUser: " + oNEA.NotifyString);
                logFileStreamWriter.Flush();
                //Console.WriteLine("** NotifyUser: " + oNEA.NotifyString);
            };
            Fiddler.FiddlerApplication.Log.OnLogString += delegate (object sender, LogEventArgs oLEA) {
                logFileStreamWriter.WriteLine("** LogString: " + oLEA.LogString);
                logFileStreamWriter.Flush();
                //Console.WriteLine("** LogString: " + oLEA.LogString);
            };

            Fiddler.FiddlerApplication.BeforeRequest += delegate (Fiddler.Session oS)
            {
                // Console.WriteLine("Before request for:\t" + oS.fullUrl);
                // In order to enable response tampering, buffering mode MUST
                // be enabled; this allows FiddlerCore to permit modification of
                // the response in the BeforeResponse handler rather than streaming
                // the response to the client as the response comes in.
                oS.bBufferResponse = false;
                Monitor.Enter(allFidlerSessions);
                allFidlerSessions.Add(oS);
                Monitor.Exit(allFidlerSessions);
                oS["X-AutoAuth"] = "(default)";

                /* If the request is going to our secure endpoint, we'll echo back the response.
                
                Note: This BeforeRequest is getting called for both our main proxy tunnel AND our secure endpoint, 
                so we have to look at which Fiddler port the client connected to (pipeClient.LocalPort) to determine whether this request 
                was sent to secure endpoint, or was merely sent to the main proxy tunnel (e.g. a CONNECT) in order to *reach* the secure endpoint.

                As a result of this, if you run the demo and visit https://localhost:7777 in your browser, you'll see

                Session list contains...
                 
                    1 CONNECT http://localhost:7777
                    200                                         <-- CONNECT tunnel sent to the main proxy tunnel, port 8877

                    2 GET https://localhost:7777/
                    200 text/html                               <-- GET request decrypted on the main proxy tunnel, port 8877

                    3 GET https://localhost:7777/               
                    200 text/html                               <-- GET request received by the secure endpoint, port 7777
                */

                //if ((oS.oRequest.pipeClient.LocalPort == iSecureEndpointPort) && (oS.hostname == sSecureEndpointHostname))
                //{
                //    oS.utilCreateResponseAndBypassServer();
                //    oS.oResponse.headers.SetStatus(200, "Ok");
                //    oS.oResponse["Content-Type"] = "text/html; charset=UTF-8";
                //    oS.oResponse["Cache-Control"] = "private, max-age=0";
                //    oS.utilSetResponseBody("<html><body>Request for httpS://" + sSecureEndpointHostname + ":" + iSecureEndpointPort.ToString() + " received. Your request was:<br /><plaintext>" + oS.oRequest.headers.ToString());
                //}
            };

            /*
                // The following event allows you to examine every response buffer read by Fiddler. Note that this isn't useful for the vast majority of
                // applications because the raw buffer is nearly useless; it's not decompressed, it includes both headers and body bytes, etc.
                //
                // This event is only useful for a handful of applications which need access to a raw, unprocessed byte-stream
                Fiddler.FiddlerApplication.OnReadResponseBuffer += new EventHandler<RawReadEventArgs>(FiddlerApplication_OnReadResponseBuffer);
            */

            Fiddler.FiddlerApplication.BeforeRequest += delegate (Fiddler.Session oS)
            {
                if (oS.hostname == "bet365" || oS.hostname == "www.bet365.com")
                {
                    oS.utilDecodeRequest();
                    oS.SaveRequest(@"C:\Users\diogo.marques\Documents\GitHubVisualStudio\WebTrafficReader\FiddlerLogFiles\" + oS.id + ".txt", false);

                    //var data = JsonConvert.SerializeObject(oS);
                    //specificLogFileStreamWriter.WriteLine(data);
                    //logFileStreamWriter.Flush();
                }
            };

            Fiddler.FiddlerApplication.BeforeResponse += delegate (Fiddler.Session oS) {
                //Console.WriteLine("{0}:HTTP {1} for {2}", oS.id, oS.responseCode, oS.fullUrl);
                logFileStreamWriter.WriteLine("{0}:HTTP {1} for {2}", oS.id, oS.responseCode, oS.fullUrl);
                logFileStreamWriter.Flush();

                if (oS.hostname == "bet365" || oS.hostname == "www.bet365.com")
                {
                    oS.utilDecodeResponse();
                    oS.SaveResponse(@"C:\Users\diogo.marques\Documents\GitHubVisualStudio\WebTrafficReader\FiddlerLogFiles\" + oS.id + ".txt", false);

                    //var data = JsonConvert.SerializeObject(oS);
                    //specificLogFileStreamWriter.WriteLine(data);
                    //logFileStreamWriter.Flush();
                }

                // Uncomment the following two statements to decompress/unchunk the
                // HTTP response and subsequently modify any HTTP responses to replace 
                // instances of the word "Microsoft" with "Bayden". You MUST also
                // set bBufferResponse = true inside the beforeREQUEST method above.
                //
                //oS.utilDecodeResponse(); oS.utilReplaceInResponse("Microsoft", "Bayden");
            };

            Fiddler.FiddlerApplication.AfterSessionComplete += delegate (Fiddler.Session oS)
            {
                if (oS.hostname == "bet365" || oS.hostname == "www.bet365.com")
                {
                    oS.utilDecodeRequest();
                    oS.utilDecodeResponse();
                    oS.SaveSession(@"C:\Users\diogo.marques\Documents\GitHubVisualStudio\WebTrafficReader\FiddlerLogFiles\" + oS.id + ".txt", false);
                    //var data = JsonConvert.SerializeObject(oS);
                    //specificLogFileStreamWriter.WriteLine(data);
                    //logFileStreamWriter.Flush();
                }

                logFileStreamWriter.WriteLine("Session list contains: " + allFidlerSessions.Count.ToString() + " sessions");
                logFileStreamWriter.Flush();
                //Console.WriteLine("Finished session:\t" + oS.fullUrl); 
                //Console.Title = ("Session list contains: " + allFidlerSessions.Count.ToString() + " sessions");
            };

            // Tell the system console to handle CTRL+C by calling our method that
            // gracefully shuts down the FiddlerCore.
            //
            // Note, this doesn't handle the case where the user closes the window with the close button.
            // See http://geekswithblogs.net/mrnat/archive/2004/09/23/11594.aspx for info on that...
            //
            //Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
            #endregion AttachEventListeners


            //Console.WriteLine(String.Format("Starting {0} ({1})...", Fiddler.FiddlerApplication.GetVersionString(), sSAZInfo));
            logFileStreamWriter.WriteLine(String.Format("Starting {0}...", Fiddler.FiddlerApplication.GetVersionString()));
            logFileStreamWriter.Flush();

            // For the purposes of this demo, we'll forbid connections to HTTPS 
            // sites that use invalid certificates. Change this from the default only
            // if you know EXACTLY what that implies.
            //Fiddler.CONFIG.IgnoreServerCertErrors = false;

            // ... but you can allow a specific (even invalid) certificate by implementing and assigning a callback...
            // FiddlerApplication.OnValidateServerCertificate += new System.EventHandler<ValidateServerCertificateEventArgs>(CheckCert);

            FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.abortifclientaborts", true);

            // For forward-compatibility with updated FiddlerCore libraries, it is strongly recommended that you 
            // start with the DEFAULT options and manually disable specific unwanted options.
            FiddlerCoreStartupFlags oFCSF = FiddlerCoreStartupFlags.Default;

            // E.g. If you want to add a flag, start with the .Default and "OR" the new flag on:
            // oFCSF = (oFCSF | FiddlerCoreStartupFlags.CaptureFTP);

            // ... or if you don't want a flag in the defaults, "and not" it out:
            // Uncomment the next line if you don't want FiddlerCore to act as the system proxy
            // oFCSF = (oFCSF & ~FiddlerCoreStartupFlags.RegisterAsSystemProxy);

            // *******************************
            // Important HTTPS Decryption Info
            // *******************************
            // When FiddlerCoreStartupFlags.DecryptSSL is enabled, you must include either
            //
            //     MakeCert.exe
            //
            // *or*
            //
            //     CertMaker.dll
            //     BCMakeCert.dll
            //
            // ... in the folder where your executable and FiddlerCore.dll live. These files
            // are needed to generate the self-signed certificates used to man-in-the-middle
            // secure traffic. MakeCert.exe uses Windows APIs to generate certificates which
            // are stored in the user's \Personal\ Certificates store. These certificates are
            // NOT compatible with iOS devices which require specific fields in the certificate
            // which are not set by MakeCert.exe. 
            //
            // In contrast, CertMaker.dll uses the BouncyCastle C# library (BCMakeCert.dll) to
            // generate new certificates from scratch. These certificates are stored in memory
            // only, and are compatible with iOS devices.

            // Uncomment the next line if you don't want to decrypt SSL traffic.
            // oFCSF = (oFCSF & ~FiddlerCoreStartupFlags.DecryptSSL);

            // NOTE: In the next line, you can pass 0 for the port (instead of 8877) to have FiddlerCore auto-select an available port
            //int iPort = 8877;
            //Fiddler.FiddlerApplication.Startup(iPort, oFCSF);
            int iPort = 0;
            Fiddler.FiddlerApplication.Startup(iPort, oFCSF);

            FiddlerApplication.Log.LogFormat("Created endpoint listening on port {0}", iPort);

            FiddlerApplication.Log.LogFormat("Starting with settings: [{0}]", oFCSF);
            FiddlerApplication.Log.LogFormat("Gateway: {0}", CONFIG.UpstreamGateway.ToString());

            //Console.WriteLine("Hit CTRL+C to end session.");
            logFileStreamWriter.WriteLine("Created endpoint listening on port {0}", iPort);
            logFileStreamWriter.WriteLine("Starting with settings: [{0}]", oFCSF);
            logFileStreamWriter.WriteLine("Gateway: {0}", CONFIG.UpstreamGateway.ToString());
            logFileStreamWriter.Flush();

            // We'll also create a HTTPS listener, useful for when FiddlerCore is masquerading as a HTTPS server
            // instead of acting as a normal CERN-style proxy server.
            //oSecureEndpoint = FiddlerApplication.CreateProxyEndpoint(iSecureEndpointPort, true, sSecureEndpointHostname);
            //if (null != oSecureEndpoint)
            //{
            //    FiddlerApplication.Log.LogFormat("Created secure endpoint listening on port {0}, using a HTTPS certificate for '{1}'", iSecureEndpointPort, sSecureEndpointHostname);
            //}

        }
    }
}
