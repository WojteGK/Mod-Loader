using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_mods_installer
{
    internal class DownloadConfig
    {        
        public Dictionary<string, bool> Mods = new()
        {
            //{ "mod-name", shouldBeInstalled? },
            
        };
        public Dictionary<string, bool> Shaders = new()
        {
            //{ "shader-name", shouldBeInstalled? },
            
        };
        public void Init(Resources existingResources){     
            if(existingResources.Mods.Count == 0){
                throw new Exception("Resources.Shaders is empty");
            }           
            foreach (Resource mod in existingResources.Mods)
            {
                Mods.Add(mod.Name, mod.IsOptional);
            }
            if(existingResources.Shaders.Count == 0){
                throw new Exception("Resources.Shaders is empty");
            }           
            foreach (Resource shader in existingResources.Shaders)
            {
                Shaders.Add(shader.Name, shader.IsOptional);
            }
        }   
    }
}
