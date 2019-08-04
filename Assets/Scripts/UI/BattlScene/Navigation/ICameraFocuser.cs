namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface ICameraFocuser
    {
        void FocusOnPlayerCruiser();
        void FocusOnPlayerNavalFactory();
        void FocusOnAICruiser();
        void FocusOnAINavalFactory();
        void FocusMidLeft();
        void FocusOnOverview();
    }
}