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

        // To avoid infinite loop of updating navigation wheel because navigation wheel just moved :P
        public override bool UpdateNavigationWheel => false;

        public override int Priority => 5;

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

            FindTarget(snapToCorners: true);
        }

        private void _navigationWheel_CenterPositionChanged(object sender, PositionChangedEventArgs e)
        {
            FindTarget(e.SnapToCorners);
        }

        private void FindTarget(bool snapToCorners)
        {
            if (snapToCorners)
            {
                Target = _navigationWheelCornersCameraTargetFinder.FindCameraTarget();
            }
            else
            {
                Target = _navigationWheelCameraTargetFinder.FindCameraTarget();
            }
        }
    }
}