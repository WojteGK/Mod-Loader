using System;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Authentication.ExtendedProtection;
using System.Text.RegularExpressions;
using System.Net;

namespace MC_mods_installer
{
    internal class Program
    {
        protected static string NevaCraftVersion = "v1.0";
        protected static string ExePath = AppDomain.CurrentDomain.BaseDirectory;
        protected static string UserName = Environment.UserName;
        protected static string RoamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        protected static string DestinationPath = $"C:\\Users\\{UserName}\\NevaCraft\\{NevaCraftVersion}";
        protected static string ModsPath = Path.Combine(DestinationPath, "mods");
        public static void InitializeRoaming(){
            string NevaCraftRoamingPath = Path.Combine(RoamingPath, $"NevaCraft\\{NevaCraftVersion}");
            if (!Directory.Exists(NevaCraftRoamingPath))
            {
                Directory.CreateDirectory(NevaCraftRoamingPath);
            }


        }
        public static void WriteColorOptions(string text, ConsoleColor textColor = ConsoleColor.White, ConsoleColor digitColor = ConsoleColor.Red)
        {
            Regex digitPattern = new Regex(@"\d");
            int leftPadding = (Console.WindowWidth - text.Length) / 2;
            string paddedText = text.PadLeft(leftPadding + text.Length);

            foreach (char character in paddedText)
            {
                if (digitPattern.IsMatch(character.ToString()))
                {
                    Console.ForegroundColor = digitColor;
                }
                else
                {
                    Console.ForegroundColor = textColor;
                }
                Console.Write(character);
            }
            Console.WriteLine();
            Console.ResetColor();
        }
        public static void DisplayMenu(bool isCleared = true)
        {
            if (isCleared) { Console.Clear(); }
            WriteColorOptions("[1] -- Install mods --", ConsoleColor.White, ConsoleColor.Green);
            WriteColorOptions("[2] - Uninstall mods -", ConsoleColor.White, ConsoleColor.Red);
            WriteColorOptions("[0] ------ Exit ------", ConsoleColor.White, ConsoleColor.Yellow);
            bool exit = false;         
            while(!exit)
            {
                var result = Console.ReadKey(true).KeyChar;
                switch (result)
                {
                    case '1':
                        throw new NotImplementedException();
                        break;
                    case '2':
                        throw new NotImplementedException();
                        break;
                    case '0':                        
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Nieznana opcja.");
                        break;
                }
            }
        }        
        static string GetExePath()
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            return exePath;
        }
        public static async Task DownloadFiles(List<Link> links, string destinationPath)
        {
            using (var httpClient = new HttpClient())
            {
                foreach (var link in links)
                {
                    var fileName = Path.GetFileName(new Uri(link.Url).AbsolutePath);
                    var destinationFilePath = Path.Combine(destinationPath, fileName);

                    var response = await httpClient.GetAsync(link.Url);

                    using (var memoryStream = await response.Content.ReadAsStreamAsync())
                    {
                        using (var fileStream = File.Create(destinationFilePath))
                        {
                            memoryStream.CopyTo(fileStream);
                        }
                    }
                }
            }
        }        
        static void Main(string[] args)
        {            
            DisplayMenu();            
            // DownloadFiles(DestinationPath); // Parameter uriString cannot be null exception
                    
        }        
    }
}
