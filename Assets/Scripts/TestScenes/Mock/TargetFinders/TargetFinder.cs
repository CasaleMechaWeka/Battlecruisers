using BattleCruisers.Buildables;
using BattleCruisers.TargetFinders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleCruisers.TestScenes.Mock
{
	public class TargetFinder : ITargetFinder
	{
		public IFactionable Target { get; set; }
		
		#pragma warning disable 67  // Unused event
		public event EventHandler<TargetEventArgs> TargetFound;
		public event EventHandler<TargetEventArgs> TargetLost;
		#pragma warning restore 67  // Unused event

		public IFactionable FindTarget()
		{
			return Target;
		}
	}
}
