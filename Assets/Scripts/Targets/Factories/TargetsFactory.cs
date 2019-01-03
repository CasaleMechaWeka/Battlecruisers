using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Targets.Factories
{
    // FELIX  Create separate factories, and contain in a TargetFactoriesProvider :)
    public class TargetsFactory : ITargetsFactory
	{
        public TargetsFactory(ICruiser enemyCruiser, IRankedTargetTracker userChosenTargetTracker)
		{
            Helper.AssertIsNotNull(enemyCruiser, userChosenTargetTracker);

            UserChosenTargetTracker = userChosenTargetTracker;

            GlobalTargetFinder globalTargetFinder = new GlobalTargetFinder(enemyCruiser);

            BomberTargetProcessor 
                = new TargetProcessor(
                    new CompositeTracker(
                        UserChosenTargetTracker,
                        new RankedTargetTracker(
                            globalTargetFinder, 
                            new BomberTargetRanker())));

            OffensiveBuildableTargetProcessor
                = new TargetProcessor(
                    new CompositeTracker(
                        UserChosenTargetTracker,
                        new RankedTargetTracker(
                            globalTargetFinder,
                            new OffensiveBuildableTargetRanker())));

            globalTargetFinder.EmitCruiserAsGlobalTarget();

            EqualTargetRanker = new EqualTargetRanker();
            ShipTargetRanker = new ShipTargetRanker();
            OffensiveBuildableTargetRanker = new OffensiveBuildableTargetRanker();
		}

        #region TargetProcessors
        public ITargetProcessor BomberTargetProcessor { get; private set; }
        public ITargetProcessor OffensiveBuildableTargetProcessor { get; private set; }

        public ITargetProcessor CreateTargetProcessor(IRankedTargetTracker rankedTargetTracker)
		{
			return new TargetProcessor(rankedTargetTracker);
		}
		#endregion TargetProcessors

		#region TargetFinders
		public ITargetFinder CreateRangedTargetFinder(ITargetDetector targetDetector, ITargetFilter targetFilter)
		{
			return new RangedTargetFinder(targetDetector, targetFilter);
		}

        public ITargetFinder CreateMinRangeTargetFinder(ITargetDetector maxRangeTargetDetector, ITargetDetector minRangeTargetDetector, ITargetFilter targetFilter)
        {
            return new MinRangeTargetFinder(maxRangeTargetDetector, minRangeTargetDetector, targetFilter);
        }

        public ITargetFinder CreateAttackingTargetFinder(IDamagable parentDamagable, ITargetFilter targetFilter)
        {
            return new AttackingTargetFinder(parentDamagable, targetFilter);
        }
        #endregion TargetFinders

        #region TargetTrackers
        public IRankedTargetTracker UserChosenTargetTracker { get; private set; }

        public ITargetTracker CreateTargetTracker(ITargetFinder targetFinder)
        {
            return new TargetTracker(targetFinder);
        }
        #endregion TargetTrackers

        #region TargetFilters
        public ITargetFilter CreateTargetFilter(Faction faction)
        {
            return new FactionTargetFilter(faction);
        }

        public ITargetFilter CreateTargetFilter(Faction faction, IList<TargetType> targetTypes)
		{
            return new FactionAndTargetTypeFilter(faction, targetTypes);
		}

		public IExactMatchTargetFilter CreateExactMatchTargetFilter()
		{
			return new ExactMatchTargetFilter();
		}

		public IExactMatchTargetFilter CreateExactMatchTargetFilter(ITarget targetToMatch)
		{
			return new ExactMatchTargetFilter() 
			{
				Target = targetToMatch
			};
		}

        public IExactMatchTargetFilter CreateMulitpleExactMatchTargetFilter()
        {
            return new MultipleExactMatchesTargetFilter();
        }

        public ITargetFilter CreateDummyTargetFilter(bool isMatchResult)
        {
            return new DummyTargetFilter(isMatchResult);
        }

        public ITargetFilter CreateTargetInFrontFilter(IUnit source)
        {
            return new TargetInFrontFilter(source);
        }
        #endregion TargetFilters

        #region Highest priority trackers
        public IRankedTargetTracker CreateUserChosenInRangeTargetTracker(ITargetTracker inRangeTargetTracker)
        {
            return new UserChosenInRangeTargetTracker(inRangeTargetTracker, UserChosenTargetTracker);
        }

        public IRankedTargetTracker CreateRankedTargetTracker(ITargetFinder targetFinder, ITargetRanker targetRanker)
        {
            return new RankedTargetTracker(targetFinder, targetRanker);
        }

        public IRankedTargetTracker CreateCompositeTracker(params IRankedTargetTracker[] targetTrackers)
        {
            return new CompositeTracker(targetTrackers);
        }
        #endregion Highest priority trackers

        #region TargetRankers
        public ITargetRanker EqualTargetRanker { get; private set; }
        public ITargetRanker ShipTargetRanker { get; private set; }
        public ITargetRanker OffensiveBuildableTargetRanker { get; private set; }

        public ITargetRanker CreateBoostedRanker(ITargetRanker baseRanker, int rankBoost)
        {
            return new BoostedRanker(baseRanker, rankBoost);
        }
        #endregion TargetRankers

        #region TargetProviders
        public ITargetProvider CreateStaticTargetProvider(ITarget target)
		{
            return new StaticTargetProvider(target);
		}

        public IBroadcastingTargetProvider CreateShipBlockingEnemyProvider(ITargetDetector enemyDetector, IUnit parentUnit)
        {
            return new ShipBlockingEnemyProvider(this, enemyDetector, parentUnit);
        }

        public IBroadcastingTargetProvider CreateShipBlockingFriendlyProvider(ITargetDetector friendlyDetector, IUnit parentUnit)
        {
            return new ShipBlockingFriendlyProvider(this, friendlyDetector, parentUnit);
        }
        #endregion TargetProviders

        #region Helpers
        public ITargetRangeHelper CreateShipRangeHelper(IShip ship)
		{
            return new ShipRangeHelper(ship);
		}
        #endregion Helpers
    }
}
