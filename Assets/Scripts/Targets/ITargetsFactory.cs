using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Targets.TargetTrackers;
using System.Collections.Generic;

namespace BattleCruisers.Targets
{
    public interface ITargetsFactory
	{
        IHighestPriorityTargetTracker UserChosenTargetTracker { get; }
		ITargetProcessor BomberTargetProcessor { get; }
		ITargetProcessor OffensiveBuildableTargetProcessor { get; }

		// Processors
		ITargetProcessor CreateTargetProcessor(IHighestPriorityTargetTracker highestPriorityTargetTracker);

		// Finders
        ITargetFinder CreateRangedTargetFinder(ITargetDetector targetDetector, ITargetFilter targetFilter);
        ITargetFinder CreateMinRangeTargetFinder(ITargetDetector maxRangeTargetDetector, ITargetDetector minRangeTargetDetector, ITargetFilter targetFilter);
        ITargetFinder CreateAttackingTargetFinder(IDamagable parentDamagable, ITargetFilter targetFilter);

        // Highest priority trackers
        IHighestPriorityTargetTracker CreateUserChosenInRangeTargetTracker(ITargetTracker inRangeTargetTracker);
        IHighestPriorityTargetTracker CreateHighestPriorityTargetTracker(ITargetFinder targetFinder, ITargetRanker targetRanker);
        IHighestPriorityTargetTracker CreateCompositeTracker(params IHighestPriorityTargetTracker[] targetTrackers);

        // Trackers
        ITargetTracker CreateTargetTracker(ITargetFinder targetFinder);

		// Filters
        ITargetFilter CreateTargetFilter(Faction faction);
		ITargetFilter CreateTargetFilter(Faction faction, IList<TargetType> targetTypes);
        ITargetFilter CreateDummyTargetFilter(bool isMatchResult);
        ITargetFilter CreateTargetInFrontFilter(IUnit source);
		IExactMatchTargetFilter CreateExactMatchTargetFilter();
		IExactMatchTargetFilter CreateExactMatchTargetFilter(ITarget targetToMatch);

		// Rankers
		ITargetRanker CreateEqualTargetRanker();
        ITargetRanker CreateShipTargetRanker();
        ITargetRanker CreateOffensiveBuildableTargetRanker();
        ITargetRanker CreateBoostedRanker(ITargetRanker baseRanker, int rankBoost);

        // Providers
        ITargetProvider CreateStaticTargetProvider(ITarget target);
        IBroadcastingTargetProvider CreateShipBlockingEnemyProvider(ITargetDetector enemyDetector, IUnit parentUnit);
        IBroadcastingTargetProvider CreateShipBlockingFriendlyProvider(ITargetDetector friendlyDetector, IUnit parentUnit);
        IHighestPriorityTargetProvider CreateHighestPriorityTargetProvider(ITargetRanker targetRanker, ITargetFilter attackingTargetFilter, IDamagable parentDamagable);

        // Helpers
        ITargetRangeHelper CreateShipRangeHelper(IShip ship);
	}
}
