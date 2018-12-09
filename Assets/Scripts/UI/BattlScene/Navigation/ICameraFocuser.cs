namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface ICameraFocuser
    {
        void FocusOnPlayerCruiser();
        void FocusOnAICruiser();
        void FocusMidLeft();
    }
}