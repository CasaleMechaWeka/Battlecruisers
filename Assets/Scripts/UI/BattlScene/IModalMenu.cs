namespace BattleCruisers.UI.BattleScene
{
    public enum UserAction
    {
        Dismissed, Quit
    }

    public delegate void MenuDismissed(UserAction UserAction);

    public interface IModalMenu
    {
        void ShowMenu(MenuDismissed onMenuDismissed);
    }
}