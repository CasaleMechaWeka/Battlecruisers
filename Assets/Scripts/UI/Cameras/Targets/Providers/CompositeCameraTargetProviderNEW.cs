using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    /// <summary>
    /// Listens for user input from multiple camera target providers.
    /// 
    /// When a provider gives input, forward that input.
    /// 
    /// When multiple providers provide input choose the provider with the highest priority.
    /// 
    /// Priorities:
    /// 4.  Scroll wheel
    /// 3.  Pinch zoom
    /// 2.  Swipe
    /// 1. Navigation wheel
    /// </summary>
    /// FELIX  Test.  Can copy legacy tests?
    public class CompositeCameraTargetProviderNEW : ICameraTargetProvider
    {
        private readonly IUserInputCameraTargetProvider _defaultTargetProvider;
        private readonly IList<IUserInputCameraTargetProvider> _targetProviders;
        private readonly ICameraTargetProvider _trumpTargetProvider;
        private readonly INavigationWheel _navigationWheel;
        private readonly ICameraNavigationWheelCalculator _navigationWheelCalculator;

        private IUserInputCameraTargetProvider _activeTargetProvider;
        private IUserInputCameraTargetProvider ActiveTargetProvider
        {
            get { return _activeTargetProvider; }
            set
            {
                Logging.Verbose(Tags.CAMERA_TARGET_PROVIDER, $"{_activeTargetProvider} > {value}");
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

        public CompositeCameraTargetProviderNEW(
            IUserInputCameraTargetProvider defaultTargetProvider,
            IList<IUserInputCameraTargetProvider> targetProviders,
            // FELIX  Can replace this with a CTP with the highest priority!  Emit events as StaticTarget is assigned :D
            ICameraTargetProvider trumpTargetProvider,
            INavigationWheel navigationWheel,
            ICameraNavigationWheelCalculator navigationWheelCalculator)
        {
            Helper.AssertIsNotNull(defaultTargetProvider, targetProviders, trumpTargetProvider, navigationWheel, navigationWheelCalculator);

            _defaultTargetProvider = defaultTargetProvider;
            _targetProviders = targetProviders;
            _trumpTargetProvider = trumpTargetProvider;
            _navigationWheel = navigationWheel;
            _navigationWheelCalculator = navigationWheelCalculator;

            ActiveTargetProvider = _defaultTargetProvider;

            foreach (IUserInputCameraTargetProvider targetProvider in _targetProviders)
            {
                targetProvider.UserInputStarted += TargetProvider_UserInputStarted;
                targetProvider.UserInputEnded += TargetProvider_UserInputEnded;
            }
        }

        private void TargetProvider_UserInputStarted(object sender, EventArgs e)
        {
            Logging.Verbose(Tags.CAMERA_TARGET_PROVIDER, $"Active target provider: {_activeTargetProvider}");

            IUserInputCameraTargetProvider startingProvider = sender.Parse<IUserInputCameraTargetProvider>();

            if (startingProvider.Priority > ActiveTargetProvider.Priority)
            {
                ActiveTargetProvider = startingProvider;
            }
        }

        private void TargetProvider_UserInputEnded(object sender, EventArgs e)
        {
            Logging.Verbose(Tags.CAMERA_TARGET_PROVIDER, $"Active target provider: {_activeTargetProvider}");

            IUserInputCameraTargetProvider endingProvider = sender.Parse<IUserInputCameraTargetProvider>();

            if (ReferenceEquals(ActiveTargetProvider, endingProvider))
            {
                Vector2 targetCenterPosition = _navigationWheelCalculator.FindNavigationWheelPosition(_activeTargetProvider.Target);
                _navigationWheel.SetCenterPosition(targetCenterPosition, snapToCorners: false);

                ActiveTargetProvider = _defaultTargetProvider;
            }
        }

        private void _activeTargetProvider_TargetChanged(object sender, EventArgs e)
        {
            TargetChanged?.Invoke(sender, e);
        }
    }
}