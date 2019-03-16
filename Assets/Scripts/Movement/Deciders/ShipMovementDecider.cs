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
        private readonly ITargetTracker _inRangeTargetTracker;
        private readonly ITargetRangeHelper _rangeHelper;

        // Frigate would have optimal range of 18.63 but target was at 18.6305.
        // Hence provide a tiny bit of leeway, so target is counted as in range.
        private const float IN_RANGE_LEEWAY_IN_M = 0.01f;

        private ITarget _highestPriorityTarget;
        public ITarget Target
        {
            set
            {
                _highestPriorityTarget = value;
                DecideMovement();
            }
        }

        public ShipMovementDecider(
            IShip ship,
            IBroadcastingTargetProvider blockingEnemyProvider,
            IBroadcastingTargetProvider blockingFriendlyProvider,
            ITargetTracker inRangeTargetTracker,
            ITargetRangeHelper rangeHelper)
        {
            Helper.AssertIsNotNull(ship, blockingEnemyProvider, blockingFriendlyProvider, inRangeTargetTracker, rangeHelper);

            _ship = ship;
            _blockingEnemyProvider = blockingEnemyProvider;
            _blockingFriendlyProvider = blockingFriendlyProvider;
            _inRangeTargetTracker = inRangeTargetTracker;
            _rangeHelper = rangeHelper;

            _blockingEnemyProvider.TargetChanged += TriggerDecideMovement;
            _blockingFriendlyProvider.TargetChanged += TriggerDecideMovement;
            _inRangeTargetTracker.TargetsChanged += TriggerDecideMovement;

            DecideMovement();
        }

        private void TriggerDecideMovement(object sender, EventArgs args)
        {
            Logging.LogDefault(Tags.SHIP_MOVEMENT_DECIDER);
            DecideMovement();
        }

        private void DecideMovement()
        {
            Logging.Log(Tags.SHIP_MOVEMENT_DECIDER, $"enemy:  {_blockingEnemyProvider.Target}  friend: {_blockingFriendlyProvider.Target}  target:  { _highestPriorityTarget}");

            if (!_ship.IsMoving)
            {
                if (_blockingEnemyProvider.Target == null
                    && _blockingFriendlyProvider.Target == null
                    && (_highestPriorityTarget == null
                        || !IsHighestPriorityTargetInRange()))
                {
                    _ship.StartMoving();
                }
            }
            else if (_blockingEnemyProvider.Target != null
                || _blockingFriendlyProvider.Target != null
                || (_highestPriorityTarget != null
                    && IsHighestPriorityTargetInRange()))
            {
                _ship.StopMoving();
            }
        }

        private bool IsHighestPriorityTargetInRange()
        {
            return _rangeHelper.IsTargetInRange(_highestPriorityTarget);
        }

        public void DisposeManagedState()
        {
            _blockingEnemyProvider.TargetChanged -= TriggerDecideMovement;
            _blockingEnemyProvider.DisposeManagedState();

            _blockingFriendlyProvider.TargetChanged -= TriggerDecideMovement;
            _blockingFriendlyProvider.DisposeManagedState();

            _inRangeTargetTracker.TargetsChanged -= TriggerDecideMovement;
            _inRangeTargetTracker.DisposeManagedState();
        }
    }
}
