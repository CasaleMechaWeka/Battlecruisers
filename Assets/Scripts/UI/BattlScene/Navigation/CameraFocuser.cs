using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    // FELIX  Update tests :)
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

        public void FocusOnPlayerCruiserDeath()
        {
            FocusCamera(_positionProvider.PlayerCruiserDeathPosition, snapToCorners: false);
        }

        public void FocusOnPlayerNavalFactory()
        {
            FocusCamera(_positionProvider.PlayerNavalFactoryPosition);
        }

        public void FocusOnAICruiser()
        {
            FocusCamera(_positionProvider.AICruiserPosition);
        }

        public void FocusOnAICruiserDeath()
        {
            FocusCamera(_positionProvider.AICruiserDeathPosition, snapToCorners: false);
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

        private void FocusCamera(Vector2 centerPosition, bool snapToCorners = true)
        {
            _navigationWheel.SetCenterPosition(centerPosition, snapToCorners);
        }
    }
}