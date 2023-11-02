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

            if (File.Exists(linksJsonPath) && File.Exists(configJsonPath))
            {
                string linksJson = File.ReadAllText(linksJsonPath);
                string configJson = File.ReadAllText(configJsonPath);

                links = JsonConvert.DeserializeObject<List<Link>>(linksJson);
                downloadOptions = JsonConvert.DeserializeObject<DownloadOptions>(configJson);
            }
            else
            {
                Console.WriteLine("Plik links.json lub config.json nie istnieje.");
            }
        }
        static void Main(string[] args)
        {

            string UserName = Environment.UserName;
            string destinationDirectory = $"C:\\Users\\{UserName}\\Downloads\\mods\\";
            
            List<Link> links;
            DownloadOptions downloadOptions;

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
        }
    }
}
