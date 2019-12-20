namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface ICameraFocuser
    {
        void FocusOnPlayerCruiser();
        void FocusOnPlayerCruiserDeath();
        void FocusOnPlayerNavalFactory();
        void FocusOnAICruiser();
        void FocusOnAICruiserDeath();
        void FocusOnAINavalFactory();
        void FocusMidLeft();
        void FocusOnOverview();
    }
}