using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.Data
{
    public class Level : ILevel
    {
		public int Num { get; private set; }
        public string Name { get; private set; }
        public IPrefabKey Hull { get; private set; }
        public ISoundKey MusicKey { get; private set; }
        public string SkyMaterialName { get; private set; }
        public ICloudGenerationStats CloudStats { get; private set; }

        public Level(
            int num, 
            string name, 
            IPrefabKey hull, 
            ISoundKey musicKey,
            string skyMaterialName, 
            ICloudGenerationStats cloudStats)
		{
            Helper.AssertIsNotNull(hull, musicKey, cloudStats);

			Num = num;
            Name = name;
            Hull = hull;
            MusicKey = musicKey;
            SkyMaterialName = skyMaterialName;
            CloudStats = cloudStats;
		}
	}
}
