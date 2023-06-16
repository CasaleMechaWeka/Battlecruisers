namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public interface IPvPCameraFocuser
    {
        void FocusOnLeftPlayerCruiser();
        void FocusOnLeftPlayerCruiserDeath();
        void FocusOnLeftPlayerCruiserNuke();
        void FocusOnLeftPlayerNavalFactory();
        void FocusOnRightPlayerCruiser();
        void FocusOnRightPlayerCruiserDeath();
        void FocusOnRightPlayerCruiserNuke();
        void FocusOnRightPlayerNavalFactory();
        void FocusMidLeft();
        void FocusOnOverview();
    }
}