using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetProviders;
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
    /// FELIX  Update tests :)
    public class ShipMovementDecider : IMovementDecider
    {
        private readonly IShip _ship;
        private readonly ITargetRangeHelper _rangeHelper;
        private readonly IBroadcastingTargetProvider _blockingEnemyProvider, _blockingFriendlyProvider;

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
            ITargetRangeHelper rangeHelper)
        {
            Helper.AssertIsNotNull(ship, blockingEnemyProvider, blockingFriendlyProvider, rangeHelper);

            _ship = ship;
            _blockingEnemyProvider = blockingEnemyProvider;
            _blockingFriendlyProvider = blockingFriendlyProvider;
            _rangeHelper = rangeHelper;

            _blockingEnemyProvider.TargetChanged += OnBlockingTargetChanged;
            _blockingFriendlyProvider.TargetChanged += OnBlockingTargetChanged;

            DecideMovement();
        }

        private void OnBlockingTargetChanged(object sender, EventArgs args)
        {
            DecideMovement();
        }

        private void DecideMovement()
        {
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
            _blockingEnemyProvider.TargetChanged -= OnBlockingTargetChanged;
            _blockingFriendlyProvider.TargetChanged -= OnBlockingTargetChanged;
        }
    }
}
