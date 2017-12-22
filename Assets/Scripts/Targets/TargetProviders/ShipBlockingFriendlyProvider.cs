using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProviders
{
    // FELIX  Test!
    public class ShipBlockingFriendlyProvider : ITargetProvider
    {
        private readonly ITargetFilter _isInFrontFilter;

        public ITarget Target { get; private set; }

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

            friendFinder.StartFindingTargets();
        }

        private void OnFriendFound(object sender, TargetEventArgs args)
        {
            Logging.Log(Tags.ATTACK_BOAT, "OnFriendFound()");

            if (_isInFrontFilter.IsMatch(args.Target))
            {
                Target = args.Target;
            }
        }

        private void OnFriendLost(object sender, TargetEventArgs args)
        {
            Logging.Log(Tags.ATTACK_BOAT, "OnFriendLost()");

            if (_isInFrontFilter.IsMatch(args.Target))
            {
                Assert.IsTrue(Target != null);

                if (object.ReferenceEquals(Target, args.Target))
                {
                    Target = null;
                }
            }
        }
    }
}
