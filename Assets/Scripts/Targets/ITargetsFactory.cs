using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Targets.TargetProviders;

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
        ITargetFinder CreateMinRangeTargetFinder(ITargetDetector maxRangeTargetDetector, ITargetDetector minRangeTargetDetector, ITargetFilter targetFilter);

        // Trackers
        ITargetTracker CreateTargetTracker(ITargetFinder targetFinder);

		// Filters
        ITargetFilter CreateTargetFilter(Faction faction);
		ITargetFilter CreateTargetFilter(Faction faction, IList<TargetType> targetTypes);
        ITargetFilter CreateDummyTargetFilter(bool isMatchResult);
		IExactMatchTargetFilter CreateExactMatchTargetFilter();
		IExactMatchTargetFilter CreateExactMatchTargetFilter(ITarget targetToMatch);

		// Rankers
		ITargetRanker CreateEqualTargetRanker();

        // Providers
        ITargetProvider CreateStaticTargetProvider(ITarget target);
        ITargetProvider CreateShipBlockingEnemyProvider(ITargetDetector enemyDetector, Faction enemyFaction);
	}
}
