using System.Collections.Generic;

namespace BattleCruisers.Data.Static
{
    public static class SkyMaterials
    {
        public const string Cold = "Skybox4-Cold";
        public const string Dusk = "Skybox6-Dusk";
        public const string Midday = "Skybox5-Midday";
        public const string Midnight = "Skybox7-Midnight";
        public const string Purple = "Skybox3-Purple";
        public const string Morning = "Skybox1-Morning";
        public const string Sunrise = "Skybox2-Sunrise";

        public static IList<string> All = new List<string>()
        {
            Cold,
            Dusk,
            Midday,
            Midnight,
            Purple,
            Morning,
            Sunrise
        };
    }
}
