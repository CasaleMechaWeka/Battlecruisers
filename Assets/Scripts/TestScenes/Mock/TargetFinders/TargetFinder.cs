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
		
		public IFactionable FindTarget()
		{
			return Target;
		}
	}
}
