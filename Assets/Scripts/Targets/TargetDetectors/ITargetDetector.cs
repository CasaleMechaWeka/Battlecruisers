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

        public ITarget Target { get; }
    }

    public interface ITargetDetector
	{
        void StartDetecting();

		event EventHandler<TargetEventArgs> TargetEntered;
		event EventHandler<TargetEventArgs> TargetExited;
	}
}
