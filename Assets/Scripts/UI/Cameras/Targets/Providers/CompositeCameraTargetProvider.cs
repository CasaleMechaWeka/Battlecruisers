using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
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
    /// (Highest)
    /// 6. Trump (static)
    /// 5. Scroll wheel
    /// 4. Pinch zoom
    /// 3. Swipe
    /// 2. Edge scrolling
    /// 1. Default (static)
    /// (Lowest)
    /// </summary>
    public class CompositeCameraTargetProvider : ICameraTargetProvider
    {
        private readonly IStaticCameraTargetProvider _defaultTargetProvider;
        private readonly IList<IUserInputCameraTargetProvider> _targetProviders;

        private IUserInputCameraTargetProvider _activeTargetProvider;
        private IUserInputCameraTargetProvider ActiveTargetProvider
        {
            get { return _activeTargetProvider; }
            set
            {
                Logging.Verbose(Tags.CAMERA_TARGET_PROVIDER, $"{_activeTargetProvider} > {value}");
                Assert.IsNotNull(value);
                Assert.IsNotNull(value.Target, $"Camera target must never be null!  Consuming code assumes the target is valid at all times.");
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
            IStaticCameraTargetProvider defaultTargetProvider,
            IList<IUserInputCameraTargetProvider> targetProviders)
        {
            Helper.AssertIsNotNull(defaultTargetProvider, targetProviders);

            _defaultTargetProvider = defaultTargetProvider;
            _targetProviders = targetProviders;

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
                _defaultTargetProvider.SetTarget(ActiveTargetProvider.Target);
                ActiveTargetProvider = _defaultTargetProvider;
            }
        }

        private void _activeTargetProvider_TargetChanged(object sender, EventArgs e)
        {
            TargetChanged?.Invoke(sender, e);
        }
    }
}