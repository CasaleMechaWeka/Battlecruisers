using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProviders;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders
{
    /// <summary>
    /// Provides a friendly target encountered right in front of the parent unit.
    /// This means the parent unit should stop moving, to not run into the blocking
    /// friendly unit.
    /// 
    /// NOTE:
    /// + Assumes not more than one friendly unit will be detected in front of the
    /// parent unit at a time (should hold true for ships :) ).
    /// </summary>
    public class PvPShipBlockingFriendlyProvider : BroadcastingTargetProvider
    {
        private readonly ITargetFinder _friendFinder;
        private readonly ITargetFilter _isInFrontFilter;

        public PvPShipBlockingFriendlyProvider(
            IPvPTargetFactoriesProvider targetsFactories,
            ITargetDetector friendDetector,
            IPvPUnit parentUnit)
        {
            PvPHelper.AssertIsNotNull(targetsFactories, friendDetector, parentUnit);

            _isInFrontFilter = targetsFactories.FilterFactory.CreateTargetInFrontFilter(parentUnit);

            IList<TargetType> blockingFriendlyTypes = new List<TargetType>() { TargetType.Ships };
            ITargetFilter friendFilter = targetsFactories.FilterFactory.CreateTargetFilter(parentUnit.Faction, blockingFriendlyTypes);
            _friendFinder = targetsFactories.FinderFactory.CreateRangedTargetFinder(friendDetector, friendFilter);

            _friendFinder.TargetFound += OnFriendFound;
            _friendFinder.TargetLost += OnFriendLost;
        }

        private void OnFriendFound(object sender, TargetEventArgs args)
        {
            // Logging.LogMethod(Tags.TARGET_PROVIDERS);

            if (!args.Target.IsDestroyed
                && _isInFrontFilter.IsMatch(args.Target))
            {
                Target = args.Target;
            }
        }

        private void OnFriendLost(object sender, TargetEventArgs args)
        {
            // Logging.LogMethod(Tags.TARGET_PROVIDERS);

            if (ReferenceEquals(Target, args.Target))
            {
                Target = null;
            }
        }

        public override void DisposeManagedState()
        {
            _friendFinder.TargetFound -= OnFriendFound;
            _friendFinder.TargetLost -= OnFriendLost;
        }
    }
}
