using BattleCruisers.TargetFinders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes.Mock
{
	public class TargetFinder : ITargetFinder
	{
		public GameObject Target { get; set; }

		public GameObject FindTarget()
		{
			return Target;
		}
	}
}
