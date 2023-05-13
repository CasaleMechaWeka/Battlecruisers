using BattleCruisers.Data.Settings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models
{
    public interface IPvPCompletedLevel
    {
        int LevelNum { get; set; }
        Difficulty HardestDifficulty { get; set; }
    }
}