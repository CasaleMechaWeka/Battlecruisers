namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public interface IPvPCameraFocuser
    {
        void FocusOnPlayerCruiser();
        void FocusOnPlayerCruiserDeath();
        void FocusOnPlayerCruiserNuke();
        void FocusOnPlayerNavalFactory();
        void FocusOnAICruiser();
        void FocusOnAICruiserDeath();
        void FocusOnAICruiserNuke();
        void FocusOnAINavalFactory();
        void FocusMidLeft();
        void FocusOnOverview();
    }
}