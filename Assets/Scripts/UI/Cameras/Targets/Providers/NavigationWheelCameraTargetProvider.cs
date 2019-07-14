using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Targets.Finders;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    /// <summary>
    /// Only finds the camera target when it has changed.  Better than polling
    /// every time we want to know the current camera target.
    /// </summary>
    /// FELIX  Update tests :)
    public class NavigationWheelCameraTargetProvider : UserInputCameraTargetProvider
    {
        private readonly INavigationWheel _navigationWheel;
        private readonly ICameraTargetFinder _navigationWheelCameraTargetFinder;

        public NavigationWheelCameraTargetProvider(
            INavigationWheel navigationWheel,
            ICameraTargetFinder navigationWheelCameraTargetFinder)
        {
            Helper.AssertIsNotNull(navigationWheel, navigationWheelCameraTargetFinder);

            _navigationWheel = navigationWheel;
            _navigationWheelCameraTargetFinder = navigationWheelCameraTargetFinder;

            _navigationWheel.CenterPositionChanged += _navigationWheel_CenterPositionChanged;

            FindTarget();
        }

        private void _navigationWheel_CenterPositionChanged(object sender, PositionChangedEventArgs e)
        {
            FindTarget();
        }

        private void FindTarget()
        {
            Target = _navigationWheelCameraTargetFinder.FindCameraTarget();
        }
    }
}