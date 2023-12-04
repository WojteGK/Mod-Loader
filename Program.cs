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
        public static void InitializeDestination()
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
        public static void LoadResources()
        {            
            string resourcesJsonFilePath = Path.Combine(ExePath, "resources.json");
            if (!File.Exists(resourcesJsonFilePath))
            {                
                
                throw new FileNotFoundException($"File was not found; Created default json instead: {resourcesJsonFilePath}.");
            }
            try
            {
                // string json = File.ReadAllText(resourcesJsonFilePath);
                // foreach (Resources mod in JsonConvert.DeserializeObject<Resources>(json))
                // {
                //     DownloadResources.Mods.Add(mod);
                // }
            }
            catch (FileNotFoundException ex)
            {
                var resources = new
                {
                    mods = DownloadResources.Mods,
                    shaders = DownloadResources.Shaders,
                    textures = DownloadResources.Textures
                };
                string json = JsonConvert.SerializeObject(resources, Formatting.Indented);
                File.WriteAllText(resourcesJsonFilePath, json);
                Console.WriteLine($"File not found: {ex.FileName}. Created default json instead: {resourcesJsonFilePath}.");
                LoadResources();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"I/O error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
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
                        InitializeRoaming();
                        InitializeDestination();
                        LoadResources();
                        DownloadMods();     
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
        public static void DownloadMods(){
            foreach (Resource mod in DownloadResources.Mods)
            {
                if (DownloadConfig.Mods[mod.Name])
                {
                    Console.WriteLine($"Downloading {mod.Name}...");
                    DownloadFileAsync(mod, ModsPath).Wait();
                }
            }
        }
        public static void DownloadShaders()
        {
            foreach (Resource shader in DownloadResources.Shaders)
            {
                if (DownloadConfig.Shaders[shader.Name])
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
