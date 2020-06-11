using System;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    // FELIX  Test :)
    public abstract class UserInputCameraTargetProvider : CameraTargetProvider, IUserInputCameraTargetProvider
    {
        private bool _duringUserInput;

        public abstract int Priority { get; }

        public event EventHandler UserInputStarted;
        public event EventHandler UserInputEnded;

        public UserInputCameraTargetProvider()
        {
            _duringUserInput = false;
        }

        protected void ReceivedUserInput()
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

        // FELIX  Remove :)
        protected void RaiseUserInputStarted()
        {
            UserInputStarted?.Invoke(this, EventArgs.Empty);
        }

        // FELIX  Remove :)
        protected void RaiseUserInputEnded()
        {
            UserInputEnded?.Invoke(this, EventArgs.Empty);
        }
    }
}