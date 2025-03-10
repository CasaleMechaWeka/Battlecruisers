namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface ICameraFocuser
    {
        void FocusOnLeftCruiser();
        void FocusOnLeftCruiserDeath();
        void FocusOnLeftCruiserNuke();
        void FocusOnLeftNavalFactory();
        void FocusOnRightCruiser();
        void FocusOnRightCruiserDeath();
        void FocusOnRightCruiserNuke();
        void FocusOnRightNavalFactory();
        void FocusMidLeft();
        void FocusOnOverview();
    }
}