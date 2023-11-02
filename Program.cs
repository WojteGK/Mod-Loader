namespace MC_mods_installer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            List<string> arguments = new List<string>();
            arguments.Add("https://www.curseforge.com/minecraft/mc-mods/torch-hit/download/4835473");
            startInfo.Arguments = $"/C {args}";
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}