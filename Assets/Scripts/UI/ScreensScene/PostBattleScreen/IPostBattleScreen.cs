using BattleCruisers.Data.Skirmishes;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public interface IPostBattleScreen
    {
        void GoToHomeScreen();
        void GoToLoadoutScreen();
        void GoToChooseDifficultyScreen();
        void Retry();
        void RetryTutorial();
        void RetrySkirmish();
        void StartLevel1();
    }
}