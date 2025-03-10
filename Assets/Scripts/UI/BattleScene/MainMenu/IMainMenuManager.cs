namespace BattleCruisers.UI.BattleScene.MainMenu
{
    public interface IMainMenuManager : IDismissableEmitter
    {
        bool IsShown { get; }

        void ShowMenu();
        void DismissMenu();
        void QuitGame();
        void RetryLevel();
        void ShowSettings();
    }
}