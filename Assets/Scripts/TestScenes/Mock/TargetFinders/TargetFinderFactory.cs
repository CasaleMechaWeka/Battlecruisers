using BattleCruisers.TargetFinders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes.Mock
{
	public class TargetFinderFactory : ITargetFinderFactory
	{
		public ITargetFinder BomberTargetFinder { get; set; }
	}
}