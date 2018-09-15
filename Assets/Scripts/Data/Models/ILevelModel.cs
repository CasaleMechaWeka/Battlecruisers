using BattleCruisers.Data.Settings;

namespace BattleCruisers.Data.Models
{
    public interface ILevelModel
    {
        int LevelNum { get; set;  }
        Difficulty HardestCompletedDifficulty { get; set; }
    }
}