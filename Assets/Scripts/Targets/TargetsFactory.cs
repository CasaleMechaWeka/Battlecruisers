using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets
{
    public class TargetsFactory : ITargetsFactory
	{
        private readonly ITargetProvider _userChosenTargetProvider;

        public ITargetProcessor BomberTargetProcessor { get; private set; }
		public ITargetProcessor OffensiveBuildableTargetProcessor { get; private set; }

		public TargetsFactory(ICruiser enemyCruiser, ITargetProvider userChosenTargetProvider)
		{
            Helper.AssertIsNotNull(enemyCruiser, userChosenTargetProvider);
            _userChosenTargetProvider = userChosenTargetProvider;

            // Global target finders keep track of what buildings th enemy cruiser builds,
            // so we must start them BEFORE the cruiser builds any buildings, otherwise
            // these targets will be lost.

			BomberTargetProcessor 
                = new TargetProcessor(
                    new HighestPriorityTargetTracker(
                        new GlobalTargetFinder(enemyCruiser), 
                        new BomberTargetRanker(_userChosenTargetProvider)));
            BomberTargetProcessor.StartProcessingTargets();

            OffensiveBuildableTargetProcessor
                = new TargetProcessor(
                    new HighestPriorityTargetTracker(
                        new GlobalTargetFinder(enemyCruiser),
                        new OffensiveBuildableTargetRanker(_userChosenTargetProvider)));
            OffensiveBuildableTargetProcessor.StartProcessingTargets();
		}

		#region TargetProcessors
		public ITargetProcessor CreateTargetProcessor(IHighestPriorityTargetTracker highestPriorityTargetTracker)
		{
			return new TargetProcessor(highestPriorityTargetTracker);
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
        #endregion TargetFinders

        #region TargetTrackers
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
        public IHighestPriorityTargetTracker CreateHighestPriorityTargetTracker(ITargetFinder targetFinder, ITargetRanker targetRanker)
        {
            return new HighestPriorityTargetTracker(targetFinder, targetRanker);
        }
        #endregion Highest priority trackers

        #region TargetRankers
        public ITargetRanker CreateEqualTargetRanker()
		{
			return new EqualTargetRanker();
		}

        public ITargetRanker CreateShipTargetRanker()
        {
            return new ShipTargetRanker(_userChosenTargetProvider);
        }

        public ITargetRanker CreateOffensiveBuildableTargetRanker()
        {
            return new OffensiveBuildableTargetRanker(_userChosenTargetProvider);
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

        public IHighestPriorityTargetProvider CreateHighestPriorityTargetProvider(ITargetRanker targetRanker, ITargetFilter attackingTargetFilter, IDamagable parentDamagable)
        {
            return new HighestPriorityTargetProvider(targetRanker, attackingTargetFilter, parentDamagable);
        }
        #endregion TargetProviders
		
		public ITargetRangeHelper CreateShipRangeHelper(IShip ship)
		{
            return new ShipRangeHelper(ship);
		}
    }
}
