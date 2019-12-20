namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface ICameraFocuser
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