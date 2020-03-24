using System;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    public abstract class UserInputCameraTargetProvider : CameraTargetProvider, IUserInputCameraTargetProvider
    {
        public abstract int Priority { get; }
        public virtual bool UpdateNavigationWheel => true;

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