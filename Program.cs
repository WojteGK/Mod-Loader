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
        public static Resources Resources = new Resources();
        public static DownloadConfig Config = new DownloadConfig();
        public static void InitializeRoaming(){
            string NevaCraftRoamingPath = Path.Combine(RoamingPath, $"NevaCraft\\{NevaCraftVersion}");
            if (!Directory.Exists(NevaCraftRoamingPath))
            {
                Directory.CreateDirectory(NevaCraftRoamingPath);
            }
        }
        protected static void InitializeDestination()
        {
            if (!Directory.Exists(DestinationPath))
            {
                Directory.CreateDirectory(DestinationPath);
            }
            if (!Directory.Exists(ModsPath))
            {
                Directory.CreateDirectory(ModsPath);
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
            Resources res = Resources;
            DownloadConfig config = Config;
            if (isCleared) { Console.Clear(); }
            WriteColorOptions("[1] -- Install mods --", ConsoleColor.White, ConsoleColor.Green);
            WriteColorOptions("[2] - Uninstall mods -", ConsoleColor.White, ConsoleColor.Red);
            WriteColorOptions("[5] - Reload console -", ConsoleColor.White, ConsoleColor.Blue);
            WriteColorOptions("[0] ------ Exit ------", ConsoleColor.White, ConsoleColor.Yellow);
            bool exit = false;         
            while(!exit)
            {
                var result = Console.ReadKey(true).KeyChar;
                switch (result)
                {
                    case '1':
                        InitializeRoaming();
                        InitializeDestination();
                        res.LoadResources(ExePath);
                        config.Init(res);
                        DownloadMods(res);                        
                        break;
                    case '2':
                        throw new NotImplementedException("Uninstalling mods is not implemented yet.");
                        // break;
                    case '5':
                        DisplayMenu(true);
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
        private static string GetExePath()
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            return exePath;
        }
        public static void DownloadMods(Resources res){            
            foreach (Resource mod in res.Mods)
            {
                if (Config.Mods[mod.Name])
                {
                    Console.WriteLine($"Downloading {mod.Name}...");
                    DownloadFileAsync(mod, ModsPath).Wait();
                }
            }
        }
        public static void DownloadShaders()
        {
            foreach (Resource shader in DefaultResources.Shaders)
            {
                if (Config.Shaders[shader.Name])
                {
                    Console.WriteLine($"Downloading {shader.Name}...");
                    DownloadFileAsync(shader, ModsPath).Wait();
                }
            }
        }
        public static async Task DownloadFileAsync(Resource resource, string destinationPath)
        {
            using (var httpClient = new HttpClient())
            {
                var fileName = "";
                try
                {
                    fileName = Path.GetFileName(new Uri(resource.Url).AbsolutePath);
                }
                catch (UriFormatException)
                {
                    Console.WriteLine($"Invalid URL: {resource.Url}");
                    return;
                }

                var destinationFilePath = Path.Combine(destinationPath, fileName);

                try
                {
                    var response = await httpClient.GetAsync(resource.Url);

                    using (var ms = await response.Content.ReadAsStreamAsync())
                    {
                        using (var fs = File.Create(destinationFilePath))
                        {
                            ms.CopyTo(fs);
                        }
                    }
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine($"Failed to download file from URL: {resource.Url}");
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