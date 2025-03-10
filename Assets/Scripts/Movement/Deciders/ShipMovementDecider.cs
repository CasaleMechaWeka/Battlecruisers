using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Movement.Deciders
{
    /// <summary>
    /// Decides whether a ship should start or stop moving.
    /// 
    /// Ship stops moving when:
    /// 1. Blocking friendly
    /// 2. Blocking enemy
    /// 3. Our highest priority target is in range.
    /// 
    /// Otherwise ship starts moving.
    /// </summary>
    public class ShipMovementDecider : IMovementDecider
    {
        private readonly IShip _ship;
        private readonly IBroadcastingTargetProvider _blockingEnemyProvider, _blockingFriendlyProvider;
        private readonly ITargetTracker _inRangeTargetTracker, _shipBlockerTargetTracker;
        private readonly ITargetRangeHelper _rangeHelper;

        private ITarget _highestPriorityTarget;
        public ITarget Target
        {
            set
            {
                Logging.Log(Tags.SHIP_MOVEMENT_DECIDER, $"_highestPriorityTarget: {_highestPriorityTarget}");
                _highestPriorityTarget = value;
                DecideMovement();
            }
        }

        public ShipMovementDecider(
            IShip ship,
            IBroadcastingTargetProvider blockingEnemyProvider,
            IBroadcastingTargetProvider blockingFriendlyProvider,
            ITargetTracker inRangeTargetTracker,
            ITargetTracker shipBlockerTargetTracker,
            ITargetRangeHelper rangeHelper)
        {
            Helper.AssertIsNotNull(ship, blockingEnemyProvider, blockingFriendlyProvider, inRangeTargetTracker, shipBlockerTargetTracker, rangeHelper);

            _ship = ship;
            _blockingEnemyProvider = blockingEnemyProvider;
            _blockingFriendlyProvider = blockingFriendlyProvider;
            _inRangeTargetTracker = inRangeTargetTracker;
            _shipBlockerTargetTracker = shipBlockerTargetTracker;
            _rangeHelper = rangeHelper;

            _blockingEnemyProvider.TargetChanged += TriggerDecideMovement;
            _blockingFriendlyProvider.TargetChanged += TriggerDecideMovement;
            _inRangeTargetTracker.TargetsChanged += TriggerDecideMovement;
            _shipBlockerTargetTracker.TargetsChanged += TriggerDecideMovement;

            DecideMovement();
        }

        private void TriggerDecideMovement(object sender, EventArgs args)
        {
            Logging.Log(Tags.SHIP_MOVEMENT_DECIDER, $"sender: {sender}");
            DecideMovement();
        }

        private void DecideMovement()
        {
            Logging.Log(Tags.SHIP_MOVEMENT_DECIDER,
                $"enemy:  {_blockingEnemyProvider.Target}"
                + $"  friend: {_blockingFriendlyProvider.Target}"
                + $"  HaveReachedEnemyCruiser: {HaveReachedEnemyCruiser()}"
                + $"  target:  { _highestPriorityTarget}"
                + $"  IsHighestPriorityTargetInRange: {IsHighestPriorityTargetInRange()}");

            if (!_ship.IsMoving)
            {
                if (_blockingEnemyProvider.Target == null
                    && _blockingFriendlyProvider.Target == null
                    && !HaveReachedEnemyCruiser()
                    && (_highestPriorityTarget == null
                        || !IsHighestPriorityTargetInRange()))
                {
                    _ship.StartMoving();
                }
            }
            else if (_blockingEnemyProvider.Target != null
                || _blockingFriendlyProvider.Target != null
                || HaveReachedEnemyCruiser()
                || (_highestPriorityTarget != null
                    && IsHighestPriorityTargetInRange()))
            {
                _ship.StopMoving();
            }
        }

        private bool IsHighestPriorityTargetInRange()
        {
            return 
                _highestPriorityTarget != null
                && _rangeHelper.IsTargetInRange(_highestPriorityTarget);
        }

        private bool HaveReachedEnemyCruiser()
        {
            return _shipBlockerTargetTracker.ContainsTarget(_ship);
        }

        public void DisposeManagedState()
        {
            _blockingEnemyProvider.TargetChanged -= TriggerDecideMovement;
            _blockingEnemyProvider.DisposeManagedState();

            _blockingFriendlyProvider.TargetChanged -= TriggerDecideMovement;
            _blockingFriendlyProvider.DisposeManagedState();

            _inRangeTargetTracker.TargetsChanged -= TriggerDecideMovement;
            _inRangeTargetTracker.DisposeManagedState();

            // Do not dispose, because owned by enemy cruiser.  Shared by all friendly ships.
            _shipBlockerTargetTracker.TargetsChanged -= TriggerDecideMovement;
        }
    }
}
