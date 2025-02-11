using BattleCruisers.UI.BattleScene.Navigation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public interface IPvPNavigationPermitterManager
    {
        NavigationPermittersState PauseNavigation();
        void RestoreNavigation(NavigationPermittersState state);
    }
}