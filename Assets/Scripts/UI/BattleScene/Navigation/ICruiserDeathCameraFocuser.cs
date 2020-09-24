using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface ICruiserDeathCameraFocuser
    {
        void FocusOnLosingCruiser(ICruiser losingCruiser);
    }
}