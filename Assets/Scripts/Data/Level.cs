using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.Data
{
    public class Level
    {
        public int Num { get; }
        public IPrefabKey Hull { get; }
        public SoundKeyPair MusicKeys { get; }
        public string SkyMaterialName { get; }
        public IPrefabKey Captains { get; }
        public HeckleConfig HeckleConfig { get; }

        public Level(
            int num,
            IPrefabKey hull,
            SoundKeyPair musicKeys,
            string skyMaterialName,
            IPrefabKey captain,
            HeckleConfig heckleConfig = null)
        {
            Helper.AssertIsNotNull(hull, musicKeys);

            Num = num;
            Hull = hull;
            MusicKeys = musicKeys;
            SkyMaterialName = skyMaterialName;
            Captains = captain;
            HeckleConfig = heckleConfig ?? new HeckleConfig(); // Default to disabled if not provided
        }
    }
}
