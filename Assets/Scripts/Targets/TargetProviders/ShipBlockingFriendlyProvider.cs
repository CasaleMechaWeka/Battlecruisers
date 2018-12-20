using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Targets.TargetProviders
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
    public class ShipBlockingFriendlyProvider : BroadcastingTargetProvider
    {
        private readonly ITargetFinder _friendFinder;
        private readonly ITargetFilter _isInFrontFilter;

        public ShipBlockingFriendlyProvider(
			ITargetsFactory targetsFactory, 
            ITargetDetector friendDetector, 
            IUnit parentUnit)
        {
            Helper.AssertIsNotNull(targetsFactory, friendDetector, parentUnit);

            _isInFrontFilter = targetsFactory.CreateTargetInFrontFilter(parentUnit);

            IList<TargetType> blockingFriendlyTypes = new List<TargetType>() { TargetType.Ships };
            ITargetFilter friendFilter = targetsFactory.CreateTargetFilter(parentUnit.Faction, blockingFriendlyTypes);
            _friendFinder = targetsFactory.CreateRangedTargetFinder(friendDetector, friendFilter);

            _friendFinder.TargetFound += OnFriendFound;
            _friendFinder.TargetLost += OnFriendLost;
        }

        private void OnFriendFound(object sender, TargetEventArgs args)
        {
            Logging.Log(Tags.TARGET_PROVIDERS, "OnFriendFound()");

            if (!args.Target.IsDestroyed
                && _isInFrontFilter.IsMatch(args.Target))
            {
                Target = args.Target;
            }
        }

        private void OnFriendLost(object sender, TargetEventArgs args)
        {
            Logging.Log(Tags.TARGET_PROVIDERS, "OnFriendLost()");

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
