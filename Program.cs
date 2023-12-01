using System;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Authentication.ExtendedProtection;
using System.Text.RegularExpressions;

namespace MC_mods_installer
{
    internal class Program
    {
        protected static string ExePath = AppDomain.CurrentDomain.BaseDirectory;
        protected static string UserName = Environment.UserName;
        protected static string RoamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        protected static string DestinationPath = $"C:\\Users\\{UserName}\\NevaCraft";
        protected static string ModsPath = Path.Combine(DestinationPath, "mods");
        public static void InitializeRoaming(){
            string NevaCraftRoamingPath = Path.Combine(RoamingPath, "NevaCraft");
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
            WriteColorOptions("[1] -- Install mods --", ConsoleColor.White, ConsoleColor.Yellow);
            WriteColorOptions("[2] - Uninstall mods -", ConsoleColor.White, ConsoleColor.Yellow);
            WriteColorOptions("[0] ------ Exit ------", ConsoleColor.White, ConsoleColor.Red);
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

        protected static void ReadFiles(out List<Link> links, out DownloadOptions downloadOptions)
        {
            links = null;
            downloadOptions = null;

            string exePath = ExePath;

            if (string.IsNullOrEmpty(exePath))
            {
                Console.WriteLine("Ścieżka do pliku wykonywalnego jest pusta.");
                return;
            }

            string directoryPath = Path.GetDirectoryName(exePath);

            if (string.IsNullOrEmpty(directoryPath))
            {
                Console.WriteLine("Ścieżka do katalogu zawierającego plik wykonywalny jest pusta.");
                return;
            }

            string linksJsonPath = Path.Combine(directoryPath, "links.json");
            string configJsonPath = Path.Combine(directoryPath, "config.json");

            try
            {
                if (File.Exists(linksJsonPath))
                {
                    string linksJson = File.ReadAllText(linksJsonPath);
                    links = JsonConvert.DeserializeObject<List<Link>>(linksJson);
                }
                else
                {
                    Console.WriteLine("Plik links.json nie istnieje.");
                }

                if (File.Exists(configJsonPath))
                {
                    string configJson = File.ReadAllText(configJsonPath);
                    downloadOptions = JsonConvert.DeserializeObject<DownloadOptions>(configJson);
                }
                else
                {
                    Console.WriteLine("Plik config.json nie istnieje.");
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Błąd deserializacji JSON: {ex.Message}");                
            }
        }        
        static string GetExePath()
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            return exePath;
        }
        static void DownloadFiles(string destinationPath){
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }
            List<Link>? links = null;
            DownloadOptions? downloadOptions = null;
            ReadFiles(out links, out downloadOptions);
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            if (links != null && downloadOptions != null)
            {
                foreach (var link in links)
                {
                    if (!link.IsOptional || (link.IsOptional && downloadOptions.Files.ContainsKey(link.Url) && downloadOptions.Files[link.Url]))
                    {
                        string fileName = Path.GetFileName(new Uri(link.Url).LocalPath);
                        string filePath = Path.Combine(DestinationPath, fileName);

                        Process process = new Process();
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        process.StartInfo = startInfo;
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo.FileName = "cmd.exe";
                        startInfo.Arguments = $"/C curl -o \"{filePath}\" {link.Url}";                        
                        process.Start();
                        process.WaitForExit();

                        if (File.Exists(filePath))
                        {
                            Console.WriteLine($"Plik {fileName} został pomyślnie pobrany i zapisany w odpowiednim folderze.");
                        }
                        else
                        {
                            Console.WriteLine($"Wystąpił błąd podczas pobierania pliku {fileName}.");
                        }
                    }
                }
            }
            else
            {                
                Console.WriteLine("Nie udało się wczytać plików links.json i config.json.");
            }
            Console.ReadKey();
        }
        static void Main(string[] args)
        {            
            DisplayMenu();            
            // DownloadFiles(DestinationPath); // Parameter uriString cannot be null exception
                    
        }        
    }
}
