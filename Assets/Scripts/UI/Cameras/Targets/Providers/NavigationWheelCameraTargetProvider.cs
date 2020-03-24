using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Targets.Finders;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Timers;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    /// <summary>
    /// Only finds the camera target when it has changed.  Better than polling
    /// every time we want to know the current camera target.
    /// </summary>
    /// // FELIX  Update tests :)
    public class NavigationWheelCameraTargetProvider : UserInputCameraTargetProvider
    {
        private readonly INavigationWheel _navigationWheel;
        private readonly ICameraTargetFinder _navigationWheelCameraTargetFinder, _navigationWheelCornersCameraTargetFinder;
        private readonly IDebouncer _inputEndedDebouncer;
        private bool _duringUserInput;

        public NavigationWheelCameraTargetProvider(
            INavigationWheel navigationWheel,
            ICameraTargetFinder navigationWheelCameraTargetFinder,
            ICameraTargetFinder navigationWheelCornersCameraTargetFinder,
            IDebouncer inputEndedDebouncer)
        {
            Helper.AssertIsNotNull(navigationWheel, navigationWheelCameraTargetFinder, navigationWheelCornersCameraTargetFinder, inputEndedDebouncer);

            _navigationWheel = navigationWheel;
            _navigationWheelCameraTargetFinder = navigationWheelCameraTargetFinder;
            _navigationWheelCornersCameraTargetFinder = navigationWheelCornersCameraTargetFinder;
            _inputEndedDebouncer = inputEndedDebouncer;

            _duringUserInput = false;
            _navigationWheel.CenterPositionChanged += _navigationWheel_CenterPositionChanged;

            FindTarget(snapToCorners: true);
        }

        private void _navigationWheel_CenterPositionChanged(object sender, PositionChangedEventArgs e)
        {
            if (!_duringUserInput)
            {
                _duringUserInput = true;
                RaiseUserInputStarted();
            }

            FindTarget(e.SnapToCorners);

            _inputEndedDebouncer.Debounce(OnUserInputEnded);
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

        /// <summary>
        /// Want to debounce this because otherwise every time the navigation wheel moved
        /// we would have a UserInput-Startd and -Ended event pair.
        /// </summary>
        private void OnUserInputEnded()
        {
            _duringUserInput = false;
            RaiseUserInputEnded();
        }
    }
}