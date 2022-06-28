using System;
using System.IO;
using System.Linq;

namespace DBD_Epic_Session_Verification
{
    public static class Globals
    {
        public static readonly string SelfExecutableName = AppDomain.CurrentDomain.FriendlyName;
        public static readonly string SelfExecutableFriendlyName = SelfExecutableName.Remove(Globals.SelfExecutableName.Length - 4, 4);
        public static readonly string SelfDataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Epic Session Verification";
        public static void EnsureSelfDataFolderExists()
        {
            if (Directory.Exists(SelfDataFolder) == false)
                Directory.CreateDirectory(SelfDataFolder);
        }

        public static string[] hostNames =
        {
            "steam.live.bhvrdbd.com",
            "brill.live.bhvrdbd.com",
            "cdn.live.dbd.bhvronline.com",
            "cdn.live.bhvrdbd.com"
        };
        public static string bhvrSession = null;
    }
}
