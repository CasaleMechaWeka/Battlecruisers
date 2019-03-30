using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    // FELIX  Test :D
    public class CompositeCameraTargetProvider : ICameraTargetProvider
    {
        private readonly ICameraTargetProvider _navigationWheelTargetProvider;
        private readonly IScrollWheelCameraTargetProvider _scrollWheelTargetProvider;
        private readonly INavigationWheel _navigationWheel;
        private readonly ICameraNavigationWheelCalculator _navigationWheelCalculator;

        private ICameraTargetProvider _activeTargetProvider;
        private ICameraTargetProvider ActiveTargetProvider
        {
            get { return _activeTargetProvider; }
            set
            {
                Assert.IsNotNull(value);
                Assert.IsFalse(ReferenceEquals(_activeTargetProvider, value));

                if (_activeTargetProvider != null)
                {
                    _activeTargetProvider.TargetChanged += _activeTargetProvider_TargetChanged;
                }

                _activeTargetProvider = value;
                _activeTargetProvider.TargetChanged += _activeTargetProvider_TargetChanged;
            }
        }

        public ICameraTarget Target => ActiveTargetProvider.Target;

        public event EventHandler TargetChanged;

        public CompositeCameraTargetProvider(
            ICameraTargetProvider navigationWheelTargetProvider,
            IScrollWheelCameraTargetProvider scrollWheelTargetProvider,
            INavigationWheel navigationWheel,
            ICameraNavigationWheelCalculator navigationWheelCalculator)
        {
            Helper.AssertIsNotNull(navigationWheelTargetProvider, scrollWheelTargetProvider, navigationWheel, navigationWheelCalculator);

            _navigationWheelTargetProvider = navigationWheelTargetProvider;
            _scrollWheelTargetProvider = scrollWheelTargetProvider;
            _navigationWheel = navigationWheel;
            _navigationWheelCalculator = navigationWheelCalculator;

            ActiveTargetProvider = navigationWheelTargetProvider;

            scrollWheelTargetProvider.UserInputStarted += ScrollWheelTargetProvider_UserInputStarted;
            scrollWheelTargetProvider.UserInputEnded += ScrollWheelTargetProvider_UserInputEnded;
        }

        private void ScrollWheelTargetProvider_UserInputStarted(object sender, EventArgs e)
        {
            ActiveTargetProvider = _scrollWheelTargetProvider;
        }

        private void ScrollWheelTargetProvider_UserInputEnded(object sender, EventArgs e)
        {
            _navigationWheel.CenterPosition = _navigationWheelCalculator.FindNavigationWheelPosition(_activeTargetProvider.Target);
            ActiveTargetProvider = _navigationWheelTargetProvider;
        }

        private void _activeTargetProvider_TargetChanged(object sender, EventArgs e)
        {
            TargetChanged?.Invoke(sender, e);
        }
    }
}