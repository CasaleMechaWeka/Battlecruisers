using BattleCruisers.Data.Settings;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelInfo
    {
        public int Num { get; private set; }
        public string Name { get; private set; }
        public Difficulty? DifficultyCompleted { get; private set; }

        public LevelInfo(int num, string name, Difficulty? difficultyCompleted)
        {
            Num = num;
            Name = name;
            DifficultyCompleted = difficultyCompleted;
        }
    }
}