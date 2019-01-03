using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using System.Collections.Generic;

namespace BattleCruisers.Targets.Factories
{
    public interface ITargetsFactory
	{
		// Processors
		ITargetProcessor BomberTargetProcessor { get; }
		ITargetProcessor OffensiveBuildableTargetProcessor { get; }
		ITargetProcessor CreateTargetProcessor(IRankedTargetTracker rankedTargetTracker);

		// Finders
        ITargetFinder CreateRangedTargetFinder(ITargetDetector targetDetector, ITargetFilter targetFilter);
        ITargetFinder CreateMinRangeTargetFinder(ITargetDetector maxRangeTargetDetector, ITargetDetector minRangeTargetDetector, ITargetFilter targetFilter);
        ITargetFinder CreateAttackingTargetFinder(IDamagable parentDamagable, ITargetFilter targetFilter);

        // Highest priority trackers
        IRankedTargetTracker UserChosenTargetTracker { get; }
        IRankedTargetTracker CreateUserChosenInRangeTargetTracker(ITargetTracker inRangeTargetTracker);
        IRankedTargetTracker CreateRankedTargetTracker(ITargetFinder targetFinder, ITargetRanker targetRanker);
        IRankedTargetTracker CreateCompositeTracker(params IRankedTargetTracker[] targetTrackers);

        // Trackers
        ITargetTracker CreateTargetTracker(ITargetFinder targetFinder);

		// Filters
        ITargetFilter CreateTargetFilter(Faction faction);
		ITargetFilter CreateTargetFilter(Faction faction, IList<TargetType> targetTypes);
        ITargetFilter CreateDummyTargetFilter(bool isMatchResult);
        ITargetFilter CreateTargetInFrontFilter(IUnit source);
		IExactMatchTargetFilter CreateExactMatchTargetFilter();
		IExactMatchTargetFilter CreateExactMatchTargetFilter(ITarget targetToMatch);
        IExactMatchTargetFilter CreateMulitpleExactMatchTargetFilter();

		// Rankers
		ITargetRanker EqualTargetRanker { get; }
        ITargetRanker ShipTargetRanker { get; }
        ITargetRanker OffensiveBuildableTargetRanker { get; }
        ITargetRanker CreateBoostedRanker(ITargetRanker baseRanker, int rankBoost);

        // Providers
        ITargetProvider CreateStaticTargetProvider(ITarget target);
        IBroadcastingTargetProvider CreateShipBlockingEnemyProvider(ITargetDetector enemyDetector, IUnit parentUnit);
        IBroadcastingTargetProvider CreateShipBlockingFriendlyProvider(ITargetDetector friendlyDetector, IUnit parentUnit);

        // Helpers
        ITargetRangeHelper CreateShipRangeHelper(IShip ship);
	}
}
