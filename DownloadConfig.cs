using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_mods_installer
{
    internal static class DownloadConfig
    {
        public static Dictionary<string, bool> Mods = new()
        {
            //{ "mod-name", shouldBeInstalled? },
            
        };
        public static void Init(){
            if(DownloadFiles.Mods.Count == 0){
                throw new Exception("DownloadFiles.Mods is empty");
            }            
            foreach (Resource mod in DownloadFiles.Mods)
            {
                Mods.Add(mod.Name, true);
            }
        }   
    }
}
