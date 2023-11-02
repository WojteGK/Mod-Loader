using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace MC_mods_installer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string ExePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string UserName = Environment.UserName;
            string destinationDirectory = $"C:\\Users\\{UserName}\\Downloads\\mods\\";
            
            List<string> urls = new List<string>
            {
                "https://mediafilez.forgecdn.net/files/4835/473/torchhit-1.20.2-6.0.2.0-fabric.jar"
            };
            foreach (string url in urls)
            {
                string fileName = Path.GetFileName(new Uri(url).LocalPath);
                string destinationPath = Path.Combine(destinationDirectory, fileName);

                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = $"/C curl -o \"{destinationPath}\" {url}";
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
}
