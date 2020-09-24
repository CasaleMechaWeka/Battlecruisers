namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface INavigationPermitterManager
    {
        NavigationPermittersState PauseNavigation();
        void RestoreNavigation(NavigationPermittersState state);
    }
}