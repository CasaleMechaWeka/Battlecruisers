using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    // FELIX  Update tests?  At least the naming :P
    public class CompositeCameraTargetProvider : ICameraTargetProvider
    {
        private readonly IUserInputCameraTargetProvider _primaryTargetProvider, _secondaryTargetProvider;
        private readonly INavigationWheel _navigationWheel;
        private readonly ICameraNavigationWheelCalculator _navigationWheelCalculator;

        private IUserInputCameraTargetProvider _activeTargetProvider;
        private IUserInputCameraTargetProvider ActiveTargetProvider
        {
            get { return _activeTargetProvider; }
            set
            {
                Assert.IsNotNull(value);
                Assert.IsFalse(ReferenceEquals(_activeTargetProvider, value));

                if (_activeTargetProvider != null)
                {
                    _activeTargetProvider.TargetChanged -= _activeTargetProvider_TargetChanged;
                }

                _activeTargetProvider = value;
                _activeTargetProvider.TargetChanged += _activeTargetProvider_TargetChanged;
            }
        }

        public ICameraTarget Target => ActiveTargetProvider.Target;

        public event EventHandler TargetChanged;

        public CompositeCameraTargetProvider(
            IUserInputCameraTargetProvider primaryTargetProvider,
            IUserInputCameraTargetProvider secondaryTargetProvider,
            INavigationWheel navigationWheel,
            ICameraNavigationWheelCalculator navigationWheelCalculator)
        {
            Helper.AssertIsNotNull(primaryTargetProvider, secondaryTargetProvider, navigationWheel, navigationWheelCalculator);

            _primaryTargetProvider = primaryTargetProvider;
            _secondaryTargetProvider = secondaryTargetProvider;
            _navigationWheel = navigationWheel;
            _navigationWheelCalculator = navigationWheelCalculator;

            ActiveTargetProvider = primaryTargetProvider;

            secondaryTargetProvider.UserInputStarted += SecondaryTargetProvider_UserInputStarted;
            secondaryTargetProvider.UserInputEnded += SecondaryTargetProvider_UserInputEnded;
        }

        private void SecondaryTargetProvider_UserInputStarted(object sender, EventArgs e)
        {
            ActiveTargetProvider = _secondaryTargetProvider;
        }

        private void SecondaryTargetProvider_UserInputEnded(object sender, EventArgs e)
        {
            _navigationWheel.SetCenterPosition(_navigationWheelCalculator.FindNavigationWheelPosition(_activeTargetProvider.Target));
            ActiveTargetProvider = _primaryTargetProvider;
        }

        private void _activeTargetProvider_TargetChanged(object sender, EventArgs e)
        {
            TargetChanged?.Invoke(sender, e);
        }
    }
}