using BattleCruisers.Cruisers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Targets.TargetFinders
{
	public interface ITargetFinderFactory
	{
		ITargetFinder BomberTargetFinder { get; }
		ITargetFinder OffensiveBuildingTargetFinder { get; }
	}

	public class TargetFinderFactory : ITargetFinderFactory
	{
		public ITargetFinder BomberTargetFinder { get; private set; }
		public ITargetFinder OffensiveBuildingTargetFinder { get; private set; }

		public TargetFinderFactory(Cruiser enemyCruiser)
		{
			BomberTargetFinder = new BomberTargetFinder(enemyCruiser);
			OffensiveBuildingTargetFinder = new OffensiveBuildingTargetFinder(enemyCruiser);
		}
	}
}
