using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.Utils;

namespace BattleCruisers.Data
{
    public class Level : ILevel
    {
		public int Num { get; private set; }
        public string Name { get; private set; }
        public IPrefabKey Hull { get; private set; }
        public string SkyMaterialName { get; private set; }
        public ICloudGenerationStats CloudStats { get; private set; }

        public Level(
            int num, 
            string name, 
            IPrefabKey hull, 
            string skyMaterialName, 
            ICloudGenerationStats cloudStats)
		{
            Helper.AssertIsNotNull(hull, cloudStats);

			Num = num;
            Name = name;
            Hull = hull;
            SkyMaterialName = skyMaterialName;
            CloudStats = cloudStats;
		}
	}
}
