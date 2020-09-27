using BattleCruisers.Data.Settings;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelInfo
    {
        public int Num { get; }
        public Difficulty? DifficultyCompleted { get; }

        public LevelInfo(int num, Difficulty? difficultyCompleted)
        {
            Num = num;
            DifficultyCompleted = difficultyCompleted;
        }
    }
}