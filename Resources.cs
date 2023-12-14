using System;
using System.Collections.Generic;

namespace MC_mods_installer
{
    public class Resources
    {
        private List<Resource> mods = new List<Resource>();
        private List<Resource> shaders = new List<Resource>();
        private List<Resource> textures = new List<Resource>();

        public List<Resource> Mods { get => mods; set => mods = value; }
        public List<Resource> Shaders { get => shaders; set => shaders = value; }
        public List<Resource> Textures { get => textures; set => textures = value; }
    }
} 