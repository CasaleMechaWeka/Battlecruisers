using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    // FELIX  Use
    // FELIX  Test :D
    public class CompositeCameraTargetProvider : ICameraTargetProvider
    {
        private readonly ICameraTargetProvider[] _targetProviders;
        private readonly INavigationWheel _navigationWheel;
        private readonly ICameraNavigationWheelCalculator _navigationWheelCalculator;

        private ICameraTargetProvider _activeTargetProvider;
        private ICameraTargetProvider ActiveTargetProvider
        {
            get { return _activeTargetProvider; }
            set
            {
                Assert.IsNotNull(value);

                if (ReferenceEquals(_activeTargetProvider, value))
                {
                    return;
                }

                _activeTargetProvider = value;

                // To avoid jumping to old navigation wheel location
                _navigationWheel.CenterPosition = _navigationWheelCalculator.FindNavigationWheelPosition(_activeTargetProvider.Target);
            }
        }

        public ICameraTarget Target => ActiveTargetProvider.Target;

        public event EventHandler TargetChanged;

        public CompositeCameraTargetProvider(
            INavigationWheel navigationWheel, 
            ICameraNavigationWheelCalculator navigationWheelCalculator,
            params ICameraTargetProvider[] targetProviders)
        {
            Helper.AssertIsNotNull(navigationWheel, navigationWheelCalculator, targetProviders);
            Assert.IsTrue(targetProviders.Length > 0);

            _targetProviders = targetProviders;
            _navigationWheel = navigationWheel;
            _navigationWheelCalculator = navigationWheelCalculator;

            _activeTargetProvider = targetProviders[0];

            foreach (ICameraTargetProvider targetProvider in targetProviders)
            {
                targetProvider.TargetChanged += TargetProvider_TargetChanged;
            }
        }

        private void TargetProvider_TargetChanged(object sender, EventArgs e)
        {
            Logging.Log(Tags.COMPOSITE_CAMERA_TARGET_PROVIDER, sender.ToString());

            ICameraTargetProvider targetProvider = sender.Parse<ICameraTargetProvider>();
            ActiveTargetProvider = targetProvider;
            TargetChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}