using BattleCruisers.Utils;
using UnityEngine;

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
            FocusCamera(_positionProvider.PlayerCruiserPosition);
        }

        public void FocusOnPlayerCruiserZoomedOut()
        {
            FocusCamera(_positionProvider.PlayerCruiserZoomedOutPosition, PositionChangeSource.Other);
        }

        public void FocusOnPlayerNavalFactory()
        {
            FocusCamera(_positionProvider.PlayerNavalFactoryPosition);
        }

        public void FocusOnAICruiser()
        {
            FocusCamera(_positionProvider.AICruiserPosition);
        }

        public void FocusOnAICruiserZoomedOut()
        {
            FocusCamera(_positionProvider.AICruiserZoomedOutPosition, PositionChangeSource.Other);
        }

        public void FocusOnAINavalFactory()
        {
            FocusCamera(_positionProvider.AINavalFactoryPosition);
        }

        public void FocusMidLeft()
        {
            FocusCamera(_positionProvider.MidLeftPosition);
        }

        public void FocusOnOverview()
        {
            FocusCamera(_positionProvider.OverviewPosition);
        }

        private void FocusCamera(Vector2 centerPosition, PositionChangeSource positionChangeSource = PositionChangeSource.CameraFocuser)
        {
            // FELIX  Do not use source, intead pass bool saying whether to snap to corners?  Or separate method?
            _navigationWheel.SetCenterPosition(centerPosition, positionChangeSource);
        }
    }
}