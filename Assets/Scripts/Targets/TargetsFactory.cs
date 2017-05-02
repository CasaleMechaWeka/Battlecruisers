using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Targets
{
	public interface ITargetsFactory
	{
		// Processors
		ITargetProcessor CreateTargetProcessor(ITargetFinder targetFinder);

		// Finders
		ITargetFinder CreateRangedTargetFinder(IFactionObjectDetector enemyDetector);

		// Filters
		ITargetFilter CreateTargetFilter(Faction faction, params TargetType[] targetTypes);
	}

	public class TargetsFactory : ITargetsFactory
	{
		public TargetsFactory(ICruiser enemyCruiser)
		{
			// FELIX  Create global target finder
		}

		#region TargetProcessors
		public ITargetProcessor CreateTargetProcessor(ITargetFinder targetFinder)
		{
			return new TargetProcessor(targetFinder);
		}
		#endregion TargetProcessors

		#region TargetFinders
		public ITargetFinder CreateRangedTargetFinder(IFactionObjectDetector enemyDetector)
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
	}
}
