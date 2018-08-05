using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

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
        private readonly ITargetFilter _isInFrontFilter;

        // FELIX  Add dispose to unsubsribe from found/lost events
        public ShipBlockingFriendlyProvider(
			ITargetsFactory targetsFactory, 
            ITargetDetector friendDetector, 
            IUnit parentUnit)
        {
            Helper.AssertIsNotNull(targetsFactory, friendDetector, parentUnit);

            _isInFrontFilter = targetsFactory.CreateTargetInFrontFilter(parentUnit);

            IList<TargetType> blockingFriendlyTypes = new List<TargetType>() { TargetType.Ships };
            ITargetFilter friendFilter = targetsFactory.CreateTargetFilter(parentUnit.Faction, blockingFriendlyTypes);
            ITargetFinder friendFinder = targetsFactory.CreateRangedTargetFinder(friendDetector, friendFilter);

            friendFinder.TargetFound += OnFriendFound;
            friendFinder.TargetLost += OnFriendLost;
        }

        private void OnFriendFound(object sender, TargetEventArgs args)
        {
            Logging.Log(Tags.TARGET_PROVIDERS, "OnFriendFound()");

            if (_isInFrontFilter.IsMatch(args.Target))
            {
                Target = args.Target;
            }
        }

        private void OnFriendLost(object sender, TargetEventArgs args)
        {
            Logging.Log(Tags.TARGET_PROVIDERS, "OnFriendLost()");

            if (_isInFrontFilter.IsMatch(args.Target))
            {
                Assert.IsTrue(Target != null);

                if (ReferenceEquals(Target, args.Target))
                {
                    Target = null;
                }
            }
        }
    }
}
