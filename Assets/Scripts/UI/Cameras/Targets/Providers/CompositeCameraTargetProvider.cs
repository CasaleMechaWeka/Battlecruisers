using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    // FELIX  Remove :)
    public class CompositeCameraTargetProvider : ICameraTargetProvider
    {
        private readonly IUserInputCameraTargetProvider _primaryTargetProvider, _secondaryTargetProvider;
        private readonly ICameraTargetProvider _trumpTargetProvider;
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

        public ICameraTarget Target => _trumpTargetProvider.Target ?? ActiveTargetProvider.Target;

        public event EventHandler TargetChanged;

        public CompositeCameraTargetProvider(
            IUserInputCameraTargetProvider primaryTargetProvider,
            IUserInputCameraTargetProvider secondaryTargetProvider,
            ICameraTargetProvider trumpTargetProvider,
            INavigationWheel navigationWheel,
            ICameraNavigationWheelCalculator navigationWheelCalculator)
        {
            Helper.AssertIsNotNull(primaryTargetProvider, secondaryTargetProvider, trumpTargetProvider, navigationWheel, navigationWheelCalculator);

            _primaryTargetProvider = primaryTargetProvider;
            _secondaryTargetProvider = secondaryTargetProvider;
            _trumpTargetProvider = trumpTargetProvider;
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
            Vector2 targetCenterPosition = _navigationWheelCalculator.FindNavigationWheelPosition(_activeTargetProvider.Target);
            _navigationWheel.SetCenterPosition(targetCenterPosition, snapToCorners: false);
            ActiveTargetProvider = _primaryTargetProvider;
        }

        private void _activeTargetProvider_TargetChanged(object sender, EventArgs e)
        {
            TargetChanged?.Invoke(sender, e);
        }
    }
}