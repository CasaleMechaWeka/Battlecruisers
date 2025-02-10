using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data
{
    public interface IPvPLevel
    {
        int Num { get; }

        IPrefabKey Hull { get; }
        SoundKeyPair MusicKeys { get; }
        string SkyMaterialName { get; }
    }
}
