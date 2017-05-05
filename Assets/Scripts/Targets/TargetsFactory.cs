using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.Targets.TargetProcessors.Ranking;

namespace BattleCruisers.Targets
{
	public interface ITargetsFactory
	{
		ITargetProcessor BomberTargetProcessor { get; }
		ITargetProcessor OffensiveTurretTargetProcessor { get; }

		// Processors
		ITargetProcessor CreateTargetProcessor(ITargetFinder targetFinder, ITargetRanker targetRanker);

		// Finders
		ITargetFinder CreateRangedTargetFinder(ITargetDetector enemyDetector);

		// Filters
		ITargetFilter CreateTargetFilter(Faction faction, params TargetType[] targetTypes);

		// Rankers
		ITargetRanker CreateEqualTargetRanker();
	}

	public class TargetsFactory : ITargetsFactory
	{
		public ITargetProcessor BomberTargetProcessor { get; private set; }
		public ITargetProcessor OffensiveTurretTargetProcessor { get; private set; }

		public TargetsFactory(ICruiser enemyCruiser)
		{
			BomberTargetProcessor = new TargetProcessor(new GlobalTargetFinder(enemyCruiser), new BomberTargetRanker());
			OffensiveTurretTargetProcessor = new TargetProcessor(new GlobalTargetFinder(enemyCruiser), new EqualTargetRanker());
		}

		#region TargetProcessors
		public ITargetProcessor CreateTargetProcessor(ITargetFinder targetFinder, ITargetRanker targetRanker)
		{
			return new TargetProcessor(targetFinder, targetRanker);
		}
		#endregion TargetProcessors

		#region TargetFinders
		public ITargetFinder CreateRangedTargetFinder(ITargetDetector enemyDetector)
		{
			return new RangedTargetFinder(enemyDetector);
		}
		#endregion TargetFinders

		#region TargetFilters
		public ITargetFilter CreateTargetFilter(Faction faction, params TargetType[] targetTypes)
		{
			return new TargetFilter(faction, targetTypes);
		}
		#endregion TargetFilters

		#region TargetRankers
		public ITargetRanker CreateEqualTargetRanker()
		{
			return new EqualTargetRanker();
		}
		#endregion TargetRankers
	}
}
