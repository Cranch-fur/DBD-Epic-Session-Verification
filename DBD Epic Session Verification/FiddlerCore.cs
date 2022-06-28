using Fiddler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

namespace DBD_Epic_Session_Verification
{
    public static class FiddlerCore
    {
        public static string rootCertificatePath = $"{Globals.SelfDataFolder}\\EpicSessionVerification_RootCertificate.p12";
        public const string rootCertificatePassword = "D944df908b82a23cea31742ec7104620";

        static FiddlerCore()
        {
            FiddlerApplication.BeforeRequest += FiddlerToCatchBeforeRequest;
            FiddlerApplication.BeforeResponse += FiddlerToCatchBeforeResponse;
            FiddlerApplication.ResetSessionCounter();
        }
        private static bool EnsureRootCertificate()
        {
            BCCertMaker.BCCertMaker certProvider = new BCCertMaker.BCCertMaker();
            CertMaker.oCertProvider = certProvider;

            if (!File.Exists(rootCertificatePath))
            {
                certProvider.CreateRootCertificate();
                certProvider.WriteRootCertificateAndPrivateKeyToPkcs12File(rootCertificatePath, rootCertificatePassword);
            }
            else certProvider.ReadRootCertificateAndPrivateKeyFromPkcs12File(rootCertificatePath, rootCertificatePassword);

            if (!CertMaker.rootCertIsTrusted())
            {
                CertMaker.trustRootCert();
            }

            return true;

        }
        public static bool DestroyRootCertificates()
        {
            CertMaker.removeFiddlerGeneratedCerts(true);
            return true;
        }



        public static bool Start()
        {
            if (EnsureRootCertificate())
                FiddlerApplication.Startup(new FiddlerCoreStartupSettingsBuilder().ListenOnPort(8866).RegisterAsSystemProxy().ChainToUpstreamGateway().DecryptSSL().OptimizeThreadPool().Build());
            return true;
        }
        public static void Stop()
        {
            FiddlerApplication.Shutdown();
        }
        public static bool GetIsRunning() { return FiddlerApplication.IsStarted(); }



        public static void FiddlerToCatchBeforeRequest(Session oSession)
        {
            if (Globals.hostNames.Contains(oSession.hostname))
            {
                oSession.bBufferResponse = true;
                if (oSession.uriContains("/api/v1/auth/provider/egs/isUnifiedAccountOnline?token="))
                {
                    oSession.utilCreateResponseAndBypassServer();
                    oSession.utilSetResponseBody("{\"online\":false}");
                    return;
                }
                
                oSession.oRequest["x-kraken-client-platform"] = "steam";
                oSession.oRequest["x-kraken-client-provider"] = "steam";
                oSession.oRequest["User-Agent"] = "DeadByDaylight/++DeadByDaylight+Live-CL-587396 EGS/10.0.19044.1.256.64bit";
            }
        }

        public static void FiddlerToCatchBeforeResponse(Session oSession)
        {
            if (Globals.hostNames.Contains(oSession.hostname))
            {
                if (oSession.uriContains("/api/v1/auth/provider/egs/login?token="))
                {
                    oSession.oResponse["Set-Cookie"] = $"bhvrSession={Globals.bhvrSession}; path=/; expires=Tue, 01 Jun 2033 20:00:00 GMT; secure; httponly";
                    return;
                }
            }
        }
    }
}
