using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.Data
{
    public class Level : ILevel
    {
		public int Num { get; }
        public IPrefabKey Hull { get; }
        public SoundKeyPair MusicKeys { get; }
        public string SkyMaterialName { get; }

        public Level(
            int num, 
            IPrefabKey hull, 
            SoundKeyPair musicKeys,
            string skyMaterialName)
		{
            Helper.AssertIsNotNull(hull, musicKeys);

			Num = num;
            Hull = hull;
            MusicKeys = musicKeys;
            SkyMaterialName = skyMaterialName;
		}
	}
}
