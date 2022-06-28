using CranchyLib.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DBD_Epic_Session_Verification
{
    class Program
    {
        private static void IncreaseConsoleBufferSize()
        {
            Console.SetIn(new StreamReader(Console.OpenStandardInput(),
                               Console.InputEncoding,
                               false,
                               bufferSize: 1024));
        }
        private static void PressEnterToContinue()
        {
            Console.WriteLine();
            Console.Write("[X] Press ENTER To Continue...");

            Console.ReadLine();
            FiddlerCore.Stop();
            Environment.Exit(0);
        }

        private static bool SpecifyBhvrSession()
        {
            Console.WriteLine();
            Console.Write("bhvrSession=");
            string _bhvrSession = Console.ReadLine();

            string[] headers =
            {
                $"Cookie: bhvrSession={_bhvrSession}",
                "Content-Type: application/json",
                "Accept-Encoding: deflate, gzip",
                "x-kraken-client-platform: steam",
                "x-kraken-client-provider: steam",
                $"x-kraken-client-resolution: {SystemParameters.PrimaryScreenWidth}x{SystemParameters.PrimaryScreenHeight}",
                $"x-kraken-client-timezone-offset: {DateTimeOffset.Now.Offset.TotalMinutes * -1}",
                "x-kraken-client-os: 10.0.19044.1.256.64bit",
                "x-kraken-client-version: 6.0.2",
                "User-Agent: DeadByDaylight/++DeadByDaylight+Live-CL-587396 Windows/10.0.19044.1.256.64bit"
            };
            var response = Networking.Request.Get("https://steam.live.bhvrdbd.com/api/v1/config", headers);
            if(response.Item1 != Networking.E_StatusCode.OK)
            {
                Console.Write($"[-] Specifed bhvrSession isn't valid! Server Response Code: {response.Item1} [{(int)response.Item1}]\n" +
                              "Press ENTER to Return To Input Screen...");
                Console.ReadLine();
                return false;
            }

            //if(response.Item3.IsJson() == false)
            //{
            //    Console.Write($"[-] Server responded with invalid Json! Server Response Code: {response.Item1} [{(int)response.Item1}]\n" +
            //                  $"Server Response: {response.Item3}\n" +
            //                  "Press ENTER to Return To Input Screen...");
            //    Console.ReadLine();
            //    return false;
            //}

            Globals.bhvrSession = _bhvrSession;
            Console.WriteLine("[+] BhvrSession has passed the check & valid, we're ready to continue...");
            return true;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("     ███████╗██████╗ ██╗ ██████╗    ███████╗███████╗███████╗███████╗██╗ ██████╗ ███╗   ██╗      \n" +
                              "     ██╔════╝██╔══██╗██║██╔════╝    ██╔════╝██╔════╝██╔════╝██╔════╝██║██╔═══██╗████╗  ██║      \n" +
                              "     █████╗  ██████╔╝██║██║         ███████╗█████╗  ███████╗███████╗██║██║   ██║██╔██╗ ██║      \n" +
                              "     ██╔══╝  ██╔═══╝ ██║██║         ╚════██║██╔══╝  ╚════██║╚════██║██║██║   ██║██║╚██╗██║      \n" +
                              "     ███████╗██║     ██║╚██████╗    ███████║███████╗███████║███████║██║╚██████╔╝██║ ╚████║      \n" +
                              "     ╚══════╝╚═╝     ╚═╝ ╚═════╝    ╚══════╝╚══════╝╚══════╝╚══════╝╚═╝ ╚═════╝ ╚═╝  ╚═══╝      \n" +
                              "                                                                                                \n" +
                              "         ██╗   ██╗███████╗██████╗ ██╗███████╗██╗ ██████╗ █████╗ ████████╗██╗ ██████╗ ███╗   ██╗ \n" +
                              "         ██║   ██║██╔════╝██╔══██╗██║██╔════╝██║██╔════╝██╔══██╗╚══██╔══╝██║██╔═══██╗████╗  ██║ \n" +
                              "         ██║   ██║█████╗  ██████╔╝██║█████╗  ██║██║     ███████║   ██║   ██║██║   ██║██╔██╗ ██║ \n" +
                              "         ╚██╗ ██╔╝██╔══╝  ██╔══██╗██║██╔══╝  ██║██║     ██╔══██║   ██║   ██║██║   ██║██║╚██╗██║ \n" +
                              "          ╚████╔╝ ███████╗██║  ██║██║██║     ██║╚██████╗██║  ██║   ██║   ██║╚██████╔╝██║ ╚████║ \n" +
                              "           ╚═══╝  ╚══════╝╚═╝  ╚═╝╚═╝╚═╝     ╚═╝ ╚═════╝╚═╝  ╚═╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝ \n" +
                              "                                                                                                \n" +
                              "              OPEN SOURCE, FREE TO USE PROJECT BY CRANCH THE WOLF & SIZZER - SERVERNAME 2022    \n");


            Console.WriteLine();
            Console.WriteLine("In case to proceed, it's required to specify bhvrSession you obtained from Steam version of the game...\n" +
                              "If you don't understand what you need to do, please, refer to the guide!");

            Globals.EnsureSelfDataFolderExists();
            IncreaseConsoleBufferSize();
#if !DEBUG
            Networking.CreateNewWebProxyInstance();
#endif
            while (SpecifyBhvrSession() == false);

            Console.WriteLine();
            Console.WriteLine("[+] Booting Up New FiddlerCore Instance!...\n" +
                              "It's required to agree with Certificate installation if Windows asks you to do so.");
            if (FiddlerCore.Start())
            {
                Console.WriteLine();
                Console.WriteLine("[+] FiddlerCore is Running!\n" +
                                  "It's about time to launch Dead By Daylight through Epic Games <:)");
                Console.WriteLine("[!] Press ENTER to Disable Fiddler Proxy & Exit from Epic Session Verification Tool");
                PressEnterToContinue();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("[-] Failed to Start FiddlerCore Instance!");
                PressEnterToContinue();
            }
        }
    }
}
