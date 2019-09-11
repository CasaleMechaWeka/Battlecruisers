using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.Data
{
    public class Level : ILevel
    {
		public int Num { get; }
        public string Name { get; }
        public IPrefabKey Hull { get; }
        public SoundKeyPair MusicKeys { get; }
        public string SkyMaterialName { get; }
        public ICloudGenerationStats CloudStats { get; }

        public Level(
            int num, 
            string name, 
            IPrefabKey hull, 
            SoundKeyPair musicKeys,
            string skyMaterialName, 
            ICloudGenerationStats cloudStats)
		{
            Helper.AssertIsNotNull(hull, musicKeys, cloudStats);

			Num = num;
            Name = name;
            Hull = hull;
            MusicKeys = musicKeys;
            SkyMaterialName = skyMaterialName;
            CloudStats = cloudStats;
		}
	}
}
