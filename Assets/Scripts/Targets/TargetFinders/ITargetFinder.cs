using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Targets.TargetFinders
{
	public class TargetEventArgs : EventArgs
	{
		public TargetEventArgs(IFactionable target)
		{
			Target = target;
		}

		public IFactionable Target { get; private set; }
	}

	/// <summary>
	/// Finds targets to feed to a ITargeProcessor.
	/// </summary>
	public interface ITargetFinder : IDisposable
	{
		// When a target is found (eg, started being built, or comes within range)
		event EventHandler<TargetEventArgs> TargetFound;

		// When an existing target is lost (eg, because it is destroyed or
		// moves out of range.
		event EventHandler<TargetEventArgs> TargetLost;

		void StartFindingTargets();
	}
}
