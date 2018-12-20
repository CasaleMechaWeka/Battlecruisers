using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.Targets.TargetDetectors
{
    public class TargetEventArgs : EventArgs
    {
        public TargetEventArgs(ITarget target)
        {
            Target = target;
        }

        public ITarget Target { get; private set; }
    }

    public interface ITargetDetector
	{
        void StartDetecting();

        // FELIX  Rename:  TargetEntered, TargetExited :)
		event EventHandler<TargetEventArgs> OnEntered;
		event EventHandler<TargetEventArgs> OnExited;
	}
}
