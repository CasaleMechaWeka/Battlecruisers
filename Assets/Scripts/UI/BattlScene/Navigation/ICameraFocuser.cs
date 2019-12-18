namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface ICameraFocuser
    {
        void FocusOnPlayerCruiser();
        void FocusOnPlayerCruiserZoomedOut();
        void FocusOnPlayerNavalFactory();
        void FocusOnAICruiser();
        void FocusOnAICruiserZoomedOut();
        void FocusOnAINavalFactory();
        void FocusMidLeft();
        void FocusOnOverview();
    }
}