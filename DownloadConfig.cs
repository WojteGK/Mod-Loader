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
        public static Dictionary<string, bool> Shaders = new()
        {
            //{ "shader-name", shouldBeInstalled? },
            
        };
        public static void Init(){
            if(DownloadResources.Mods.Count == 0){
                throw new Exception("DownloadFiles.Mods is empty");
            }            
            foreach (Resource mod in DownloadResources.Mods)
            {
                Mods.Add(mod.Name, mod.IsOptional);
            }
            if(DownloadResources.Shaders.Count == 0){
                throw new Exception("DownloadFiles.Shaders is empty");
            }           
            foreach (Resource shader in DownloadResources.Shaders)
            {
                Mods.Add(shader.Name, shader.IsOptional);
            }
        }   
    }
}
