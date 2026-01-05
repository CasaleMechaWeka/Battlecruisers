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
        public bool HasSequencer { get; }


        public Level(
            int num,
            IPrefabKey hull,
            SoundKeyPair musicKeys,
            string skyMaterialName,
            IPrefabKey captain,
            bool hasSequencer = false,
            HeckleConfig heckleConfig = null)
        {
            Helper.AssertIsNotNull(hull, musicKeys);

            Num = num;
            Hull = hull;
            MusicKeys = musicKeys;
            SkyMaterialName = skyMaterialName;
            Captains = captain;
            HeckleConfig = heckleConfig ?? new HeckleConfig
            {
                enableHeckles = true,
                maxHeckles = 3,
                minTimeBeforeFirstHeckle = 1f,
                maxTimeBeforeFirstHeckle = 60f,
                minTimeBetweenHeckles = 180f,
                // Event triggers (note: only health threshold is implemented, others are placeholders)
                heckleOnFirstDamage = false,  // Not implemented yet
                enableHealthThresholdHeckle = true,
                heckleOnHealthThreshold = 0.1f,
                heckleOnPlayerDamaged = false  // Not implemented yet
            };
            HasSequencer = hasSequencer;
        }
    }
}
