using System;
using System.Collections.Generic;

namespace MC_mods_installer
{
    public static class Resources
    {
        private static List<Resource> mods = new List<Resource>();
        private static List<Resource> shaders = new List<Resource>();
        private static List<Resource> textures = new List<Resource>();

        public static List<Resource> Mods { get => mods; set => mods = value; }
        public static List<Resource> Shaders { get => shaders; set => shaders = value; }
        public static List<Resource> Textures { get => textures; set => textures = value; }
    }
} 