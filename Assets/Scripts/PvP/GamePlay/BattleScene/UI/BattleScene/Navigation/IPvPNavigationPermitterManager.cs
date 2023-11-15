namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public interface IPvPNavigationPermitterManager
    {
        PvPNavigationPermittersState PauseNavigation();
        void RestoreNavigation(PvPNavigationPermittersState state);
    }
}