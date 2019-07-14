namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public interface IPostBattleScreen
    {
        void GoToHomeScreen();
        void GoToLoadoutScreen();
        void Retry();
        void RetryTutorial();
        void StartLevel1();
    }
}