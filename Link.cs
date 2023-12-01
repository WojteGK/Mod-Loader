using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_mods_installer
{
    internal class Link
    {
        public string? Name;
        public string? Url;
        public bool IsOptional;
        public Link(string name, string url, bool isOptional)
        {
            Name = name;
            Url = url;
            IsOptional = isOptional;
        }
    }
}
