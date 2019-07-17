using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    public abstract class UserInputCameraTargetProvider : IUserInputCameraTargetProvider
    {
        private ICameraTarget _target;
        public ICameraTarget Target
        {
            get { return _target; }
            protected set
            {
                if (!_target.SmartEquals(value))
                {
                    Logging.Log(Tags.CAMERA_TARGET_PROVIDER, $"{_target} > {value}");

                    _target = value;
                    TargetChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler TargetChanged;
        public event EventHandler UserInputStarted;
        public event EventHandler UserInputEnded;

        protected void RaiseUserInputStarted()
        {
            UserInputStarted?.Invoke(this, EventArgs.Empty);
        }

        protected void RaiseUserInputEnded()
        {
            UserInputEnded?.Invoke(this, EventArgs.Empty);
        }
    }
}