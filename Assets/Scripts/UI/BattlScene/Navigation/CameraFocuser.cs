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
            _navigationWheel.SetCenterPosition(_positionProvider.PlayerCruiserPosition, PositionChangeSource.CameraFocuser);
        }

        public void FocusOnPlayerNavalFactory()
        {
            _navigationWheel.SetCenterPosition(_positionProvider.PlayerNavalFactoryPosition, PositionChangeSource.CameraFocuser);
        }

        public void FocusOnAICruiser()
        {
            _navigationWheel.SetCenterPosition(_positionProvider.AICruiserPosition, PositionChangeSource.CameraFocuser);
        }

        public void FocusOnAINavalFactory()
        {
            _navigationWheel.SetCenterPosition(_positionProvider.AINavalFactoryPosition, PositionChangeSource.CameraFocuser);
        }

        public void FocusMidLeft()
        {
            _navigationWheel.SetCenterPosition(_positionProvider.MidLeftPosition, PositionChangeSource.CameraFocuser);
        }
    }
}