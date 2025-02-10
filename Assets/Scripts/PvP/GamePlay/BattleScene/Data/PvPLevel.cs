using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data
{
    public class PvPLevel : IPvPLevel
    {
        public int Num { get; }
        public IPrefabKey Hull { get; }
        public SoundKeyPair MusicKeys { get; }
        public string SkyMaterialName { get; }

        public PvPLevel(
            int num,
            IPrefabKey hull,
            SoundKeyPair musicKeys,
            string skyMaterialName)
        {
            PvPHelper.AssertIsNotNull(hull, musicKeys);

            Num = num;
            Hull = hull;
            MusicKeys = musicKeys;
            SkyMaterialName = skyMaterialName;
        }
    }
}
