using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data
{
    public interface IPvPLevel
    {
        int Num { get; }

        IPvPPrefabKey Hull { get; }
        PvPSoundKeyPair MusicKeys { get; }
        string SkyMaterialName { get; }
    }
}
