using BattleCruisers.Data.Settings;

namespace BattleCruisers.Data.Models
{
    public interface ICompletedLevel
    {
        int LevelNum { get; set;  }
        Difficulty HardestDifficulty { get; set; }
    }
}