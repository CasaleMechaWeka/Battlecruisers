using BattleCruisers.TargetFinders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleCruisers.TestScenes.Mock
{
	public class TargetFinder : ITargetFinder
	{
		public GameObject Target { get; set; }
		public bool IsTargetAvailable { get; set; }

		public event EventHandler TargetFound;

		public GameObject FindTarget()
		{
			return Target;
		}
	}
}
