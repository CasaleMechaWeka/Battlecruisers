using System;

namespace BattleCruisers.Targets.TargetFinders
{
	public interface ITargetDetector
	{
        void StartDetecting();

		event EventHandler<TargetEventArgs> OnEntered;
		event EventHandler<TargetEventArgs> OnExited;
	}
}
