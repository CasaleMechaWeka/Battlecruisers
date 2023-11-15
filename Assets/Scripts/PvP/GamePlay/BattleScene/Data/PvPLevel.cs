using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data
{
    public class PvPLevel : IPvPLevel
    {
        public int Num { get; }
        public IPvPPrefabKey Hull { get; }
        public PvPSoundKeyPair MusicKeys { get; }
        public string SkyMaterialName { get; }

        public PvPLevel(
            int num,
            IPvPPrefabKey hull,
            PvPSoundKeyPair musicKeys,
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
