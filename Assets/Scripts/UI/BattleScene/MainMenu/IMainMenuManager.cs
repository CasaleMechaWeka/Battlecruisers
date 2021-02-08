namespace BattleCruisers.UI.BattleScene.MainMenu
{
    public interface IMainMenuManager : IDismissableEmitter
    {
        void ShowMenu();
        void DismissMenu();
        void QuitGame();
        void RetryLevel();
        void ShowSettings();
    }
}