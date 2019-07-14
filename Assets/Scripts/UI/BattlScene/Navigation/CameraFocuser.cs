using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class CameraFocuser : ICameraFocuser
    {
        private readonly INavigationWheelPositionProvider _positionProvider;
        private readonly INavigationWheel _navigationWheel;

        public CameraFocuser(INavigationWheelPositionProvider positionProvider, INavigationWheel navigationWheel)
        {
            Helper.AssertIsNotNull(positionProvider, navigationWheel);

            _positionProvider = positionProvider;
            _navigationWheel = navigationWheel;
        }

        public void FocusOnPlayerCruiser()
        {
            _navigationWheel.SetCenterPosition(_positionProvider.PlayerCruiserPosition);
        }

        public void FocusOnPlayerNavalFactory()
        {
            _navigationWheel.SetCenterPosition(_positionProvider.PlayerNavalFactoryPosition);
        }

        public void FocusOnAICruiser()
        {
            _navigationWheel.SetCenterPosition(_positionProvider.AICruiserPosition);
        }

        public void FocusOnAINavalFactory()
        {
            _navigationWheel.SetCenterPosition(_positionProvider.AINavalFactoryPosition);
        }

        public void FocusMidLeft()
        {
            _navigationWheel.SetCenterPosition(_positionProvider.MidLeftPosition);
        }
    }
}