using System;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    public abstract class UserInputCameraTargetProvider : IUserInputCameraTargetProvider
    {
        private bool _duringUserInput;

        public abstract int Priority { get; }

        private ICameraTarget _target;
        public ICameraTarget Target
        {
            get { return _target; }
            protected set
            {
                if (_target.SmartEquals(value))
                {
                    return;
                }

                Logging.Verbose(Tags.CAMERA_TARGET_PROVIDER, $"{_target} > {value}");

                _target = value;
                ReceivedUserInput();
                TargetChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler TargetChanged;

        public event EventHandler UserInputStarted;
        public event EventHandler UserInputEnded;

        public UserInputCameraTargetProvider()
        {
            _duringUserInput = false;
        }

        private void ReceivedUserInput()
        {
            if (!_duringUserInput)
            {
                _duringUserInput = true;
                UserInputStarted?.Invoke(this, EventArgs.Empty);
            }
        }

        protected void UserInputEnd()
        {
            if (_duringUserInput)
            {
                _duringUserInput = false;
                UserInputEnded?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}