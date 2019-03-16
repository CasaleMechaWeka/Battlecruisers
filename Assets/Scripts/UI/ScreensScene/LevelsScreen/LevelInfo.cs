using BattleCruisers.Data.Settings;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelInfo
    {
        public int Num { get; }
        public string Name { get; }
        public Difficulty? DifficultyCompleted { get; }

        public LevelInfo(int num, string name, Difficulty? difficultyCompleted)
        {
            Num = num;
            Name = name;
            DifficultyCompleted = difficultyCompleted;
        }
    }
}