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
            {"torch hit", true},
            {"fastback", true},
            {"Quickplant", true},
            {"voicechat", true},
            {"combatroll", true},
            {"farsight", true},
            {"Origins", true},
            {"skyvillages", true},
            {"dungeons and taverns", true},
            {"RPGOrigins", true},
            {"fabric-release-regrowth", true},
            {"firstperson", true},
            {"darkstarter", true},
            {"rpg hud", true},
            {"cookedcarrots", true},
            {"blossom", true},
            {"strange berries", true},
            {"wardentools", true},
            {"entityculling-fabric", true},
            {"ferritecore", true},
            {"AmbientSounds", true},
            {"Terralith", true},
            {"ImmediatelyFast", true},
            {"deeperdarker-fabric", true},
            {"Fabulously.Optimized", true},
            {"Travelers-Backpack-Mod-Fabric", true}
        };       
    }
}
