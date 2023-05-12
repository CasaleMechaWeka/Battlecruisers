namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.MainMenu
{
    public interface IPvPMainMenuManager : IPvPDismissableEmitter
    {
        bool IsShown { get; }

        void ShowMenu();
        void DismissMenu();
        void QuitGame();
        void RetryLevel();
        void ShowSettings();
    }
}