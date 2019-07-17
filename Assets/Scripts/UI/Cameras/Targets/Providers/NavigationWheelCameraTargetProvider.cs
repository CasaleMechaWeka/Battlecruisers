using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Targets.Finders;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    /// <summary>
    /// Only finds the camera target when it has changed.  Better than polling
    /// every time we want to know the current camera target.
    /// </summary>
    public class NavigationWheelCameraTargetProvider : UserInputCameraTargetProvider
    {
        private readonly INavigationWheel _navigationWheel;
        private readonly ICameraTargetFinder _navigationWheelCameraTargetFinder, _navigationWheelCornersCameraTargetFinder;

        public NavigationWheelCameraTargetProvider(
            INavigationWheel navigationWheel,
            ICameraTargetFinder navigationWheelCameraTargetFinder,
            ICameraTargetFinder navigationWheelCornersCameraTargetFinder)
        {
            Helper.AssertIsNotNull(navigationWheel, navigationWheelCameraTargetFinder, navigationWheelCornersCameraTargetFinder);

            _navigationWheel = navigationWheel;
            _navigationWheelCameraTargetFinder = navigationWheelCameraTargetFinder;
            _navigationWheelCornersCameraTargetFinder = navigationWheelCornersCameraTargetFinder;

            _navigationWheel.CenterPositionChanged += _navigationWheel_CenterPositionChanged;

            FindTarget(PositionChangeSource.NavigationWheel);
        }

        private void _navigationWheel_CenterPositionChanged(object sender, PositionChangedEventArgs e)
        {
            FindTarget(e.Source);
        }

        private void FindTarget(PositionChangeSource source)
        {
            if (source == PositionChangeSource.Other)
            {
                // Do not snap to corners of navigation wheel panel.  This is to allow
                // fine grained camera movement via the mouse scroll wheel or touch swiping.
                Target = _navigationWheelCameraTargetFinder.FindCameraTarget();
            }
            else
            {
                Target = _navigationWheelCornersCameraTargetFinder.FindCameraTarget();
            }
        }
    }
}