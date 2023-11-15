using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets.Providers
{
    public abstract class PvPUserInputCameraTargetProvider : IPvPUserInputCameraTargetProvider
    {
        private bool _duringUserInput;

        public abstract int Priority { get; }

        private IPvPCameraTarget _target;
        public IPvPCameraTarget Target
        {
            get { return _target; }
            protected set
            {
                if (_target.SmartEquals(value))
                {
                    return;
                }

                // Logging.Verbose(Tags.CAMERA_TARGET_PROVIDER, $"{_target} > {value}");

                _target = value;
                ReceivedUserInput();
                TargetChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler TargetChanged;

        public event EventHandler UserInputStarted;
        public event EventHandler UserInputEnded;

        public PvPUserInputCameraTargetProvider()
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