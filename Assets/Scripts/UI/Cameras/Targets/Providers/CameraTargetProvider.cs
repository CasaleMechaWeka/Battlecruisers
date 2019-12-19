using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    public abstract class CameraTargetProvider : ICameraTargetProvider
    {
        private ICameraTarget _target;
        public ICameraTarget Target
        {
            get { return _target; }
            protected set
            {
                if (!_target.SmartEquals(value))
                {
                    Logging.Verbose(Tags.CAMERA_TARGET_PROVIDER, $"{_target} > {value}");

                    _target = value;
                    TargetChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler TargetChanged;
    }
}