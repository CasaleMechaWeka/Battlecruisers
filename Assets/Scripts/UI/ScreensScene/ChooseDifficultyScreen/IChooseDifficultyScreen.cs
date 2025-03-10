using BattleCruisers.Data.Settings;

namespace BattleCruisers.UI.ScreensScene.ChooseDifficultyScreen
{
    public interface IChooseDifficultyScreen : IDismissableEmitter
    {
        void ChooseDifficulty(Difficulty difficulty);
    }
}