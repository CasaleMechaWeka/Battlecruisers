using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TargetFinders
{
	public class TargetEventArgs : EventArgs
	{
		public TargetEventArgs(IFactionable target)
		{
			Target = target;
		}

		public IFactionable Target { get; private set; }
	}

	public interface ITargetFinder
	{
		bool IsTargetAvailable { get; }

		// When a new target is found, usually when a target comes within range.
		event EventHandler<TargetEventArgs> TargetFound;

		// When an existing target is lost, either because it moves out of
		// range or is destroyed.
		event EventHandler<TargetEventArgs> TargetLost;

		/// <returns>A target faction object, or null if no valid targets can be found.</returns>
		IFactionable FindTarget();
	}

	// FELIX  Implement properly :P
	public class BomberTargetFinder : MonoBehaviour, ITargetFinder
	{
		private Cruiser _enemyCruiser;

		public event EventHandler<TargetEventArgs> TargetFound;
		public event EventHandler<TargetEventArgs> TargetLost;

		public bool IsTargetAvailable { get { return true; } }

		public BomberTargetFinder(Cruiser enemyCruiser)
		{
			_enemyCruiser = enemyCruiser;
		}

		public IFactionable FindTarget()
		{
			return _enemyCruiser;
		}
	}
}
