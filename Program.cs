using System;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MC_mods_installer
{
    internal class Program
    {
        static string ExePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static void ReadFiles(out List<Link> links, out DownloadOptions downloadOptions)
        {
            links = null;
            downloadOptions = null;

            string linksJsonPath = Path.Combine(Path.GetDirectoryName(ExePath), "links.json");
            string configJsonPath = Path.Combine(Path.GetDirectoryName(ExePath), "config.json");

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
                // Obsłuż błąd deserializacji, na przykład informując użytkownika o problemie z plikami JSON.
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine(ExePath);
            string UserName = Environment.UserName;

            string destinationDirectory = $"C:\\Users\\{UserName}\\Downloads\\mods\\";
            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            List<Link>? links = null;
            DownloadOptions? downloadOptions = null;

            ReadFiles(out links, out downloadOptions);

            if (links != null && downloadOptions != null)
            {
                foreach (var link in links)
                {
                    if (!link.IsOptional || (link.IsOptional && downloadOptions.Files.ContainsKey(link.Url) && downloadOptions.Files[link.Url]))
                    {
                        string fileName = Path.GetFileName(new Uri(link.Url).LocalPath);
                        string destinationPath = Path.Combine(destinationDirectory, fileName);

                        Process process = new Process();
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo.FileName = "cmd.exe";
                        startInfo.Arguments = $"/C curl -o \"{destinationPath}\" {link.Url}";
                        process.StartInfo = startInfo;
                        process.Start();
                        process.WaitForExit();

                        if (File.Exists(destinationPath))
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
    }
}
