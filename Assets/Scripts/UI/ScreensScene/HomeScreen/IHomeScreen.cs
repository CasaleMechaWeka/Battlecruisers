using BattleCruisers.UI.BattleScene.Presentables;

namespace BattleCruisers.UI.ScreensScene.HomeScreen
{
    public interface IHomeScreen : IPresentable
    {
        void Continue();
        void StartLevel1();
        void StartTutorial();
        void GoToLevelsScreen();
        void GoToSettingsScreen();
        void GoToLoadoutScreen();
        void Quit();
    }
}