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
    public class NavigationWheelCameraTargetProvider : ICameraTargetProvider
    {
        private readonly INavigationWheel _navigationWheel;
        private readonly ICameraTargetFinder _navigationWheelCameraTargetFinder;

        private ICameraTarget _target;
        public ICameraTarget Target
        {
            get { return _target; }
            set
            {
                if (_target != value)
                {
                    _target = value;

                    TargetChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler TargetChanged;

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

        private void _navigationWheel_CenterPositionChanged(object sender, EventArgs e)
        {
            FindTarget();
        }

        private void FindTarget()
        {
            Target = _navigationWheelCameraTargetFinder.FindCameraTarget();
        }
    }
}