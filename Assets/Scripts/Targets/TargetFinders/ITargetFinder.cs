using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Targets.TargetFinders
{
    public class TargetEventArgs : EventArgs
	{
		public TargetEventArgs(ITarget target)
		{
			Target = target;
		}

		public ITarget Target { get; private set; }
	}

	/// <summary>
	/// Finds targets to feed to a ITargeProcessor.
	/// </summary>
	public interface ITargetFinder : IManagedDisposable
    {
		// When a target is found (eg, started being built, or comes within range)
		event EventHandler<TargetEventArgs> TargetFound;

		// When an existing target is lost (eg, because it is destroyed or
		// moves out of range)
		event EventHandler<TargetEventArgs> TargetLost;
	}
}
