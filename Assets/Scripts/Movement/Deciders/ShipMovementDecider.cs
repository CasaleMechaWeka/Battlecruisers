using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Targets;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetFinders;
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
    /// 3. Have in range target, and no higher priority target is attacking us.
    /// 
    /// Otherwise ship starts moving.
    /// </summary>
    /// FELIX  Update tests :)
    public class ShipMovementDecider : IMovementDecider
    {
        // FELIX  Inject everything :P (most things)  Should not have to use ITargetsFactory :)
        private readonly IShip _ship;
        private readonly ITargetsFactory _targetsFactory;
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
            ITargetsFactory targetsFactory,
            ITargetDetector enemyDetector,
            ITargetDetector friendDetector)
        {
            Helper.AssertIsNotNull(ship, targetsFactory, enemyDetector, friendDetector);

            _ship = ship;
            _targetsFactory = targetsFactory;

            _rangeHelper = _targetsFactory.CreateShipRangeHelper(_ship);
            _blockingEnemyProvider = SetupBlockingEnemyDetection(enemyDetector);
            _blockingFriendlyProvider = SetupBlockingFriendDetection(friendDetector);

            DecideMovement();
        }

        private IBroadcastingTargetProvider SetupBlockingEnemyDetection(ITargetDetector enemyDetector)
        {
            IBroadcastingTargetProvider blockingEnemyProvider = _targetsFactory.CreateShipBlockingEnemyProvider(enemyDetector, _ship);
            blockingEnemyProvider.TargetChanged += OnTargetChanged;
            return blockingEnemyProvider;
        }

        private IBroadcastingTargetProvider SetupBlockingFriendDetection(ITargetDetector friendDetector)
        {
            IBroadcastingTargetProvider blockingFriendlyProvider = _targetsFactory.CreateShipBlockingFriendlyProvider(friendDetector, _ship);
            blockingFriendlyProvider.TargetChanged += OnTargetChanged;
            return blockingFriendlyProvider;
        }

        private void OnTargetChanged(object sender, EventArgs args)
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
            _blockingEnemyProvider.TargetChanged -= OnTargetChanged;
            _blockingFriendlyProvider.TargetChanged -= OnTargetChanged;
        }
    }
}
