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

	public interface ITargetFinder
	{
		/// <returns>A target faction object, or null if no valid targets can be found.</returns>
		IFactionable FindTarget();
	}
}
