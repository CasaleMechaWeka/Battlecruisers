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
		ITargetProcessor OffensiveBuildableTargetProcessor { get; }

		// Processors
		ITargetProcessor CreateTargetProcessor(ITargetFinder targetFinder, ITargetRanker targetRanker);

		// Finders
		ITargetFinder CreateRangedTargetFinder(ITargetDetector targetDetector, ITargetFilter targetFilter);

		// Filters
		ITargetFilter CreateDetectableTargetFilter(Faction faction, bool isDetectable, List<TargetType> targetTypes);
		ITargetFilter CreateDetectableTargetFilter(Faction faction, bool isDetectable, params TargetType[] targetTypes);
		ITargetFilter CreateTargetFilter(Faction faction, List<TargetType> targetTypes);
		ITargetFilter CreateTargetFilter(Faction faction, params TargetType[] targetTypes);
		IExactMatchTargetFilter CreateExactMatchTargetFiler();
		IExactMatchTargetFilter CreateExactMatchTargetFiler(ITarget targetToMatch);

		// Rankers
		ITargetRanker CreateEqualTargetRanker();
	}
}
