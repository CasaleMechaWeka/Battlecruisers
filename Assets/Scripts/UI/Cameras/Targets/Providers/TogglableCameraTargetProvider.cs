using BattleCruisers.UI.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    // FELIX  Test :)
    public class TogglableCameraTargetProvider : UserInputCameraTargetProvider
    {
        private readonly IBroadcastingFilter _enabledFilter;

        public override ICameraTarget Target
        {
            get { return base.Target; }
            protected set
            {
                if (_enabledFilter.IsMatch)
                {
                    base.Target = value;
                }
            }
        }

        public TogglableCameraTargetProvider(IBroadcastingFilter enabledFilter)
        {
            Assert.IsNotNull(enabledFilter);
            _enabledFilter = enabledFilter;
        }
    }
}