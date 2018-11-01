using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI.Cameras
{
    // FELIX  Test :)
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

                    if (TargetChanged != null)
                    {
                        TargetChanged.Invoke(this, EventArgs.Empty);
                    }
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