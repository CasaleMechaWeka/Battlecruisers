using System;

namespace BattleCruisers.Targets.TargetFinders
{
	public interface ITargetDetector
	{
		event EventHandler<TargetEventArgs> OnEntered;
		event EventHandler<TargetEventArgs> OnExited;
	}
}
