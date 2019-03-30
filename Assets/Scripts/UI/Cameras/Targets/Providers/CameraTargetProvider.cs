using System;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    // FELIX  Test :P
    public abstract class CameraTargetProvider : ICameraTargetProvider
    {
        private ICameraTarget _target;
        public ICameraTarget Target
        {
            get { return _target; }
            protected set
            {
                if (_target != value)
                {
                    _target = value;

                    TargetChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler TargetChanged;
    }
}