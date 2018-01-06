using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

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
    /// Otherwise ships starts moving.
    /// </summary>
    public class ShipMovementDecider : IMovementDecider
    {
        private readonly IShip _ship;
        private readonly ITargetsFactory _targetsFactory;
        private readonly IBroadCastingTargetProvider _blockingEnemyProvider, _blockingFriendlyProvider;
        private readonly IHighestPriorityTargetProvider _highPriorityTarget;
        private readonly ITargetProvider _highestPriorityTargetProvider;
        private readonly ITargetConsumer _highestPriorityTargetConsumer;

        // Frigate would have optimal range of 18.63 but target was at 18.6305.
        // Hence provide a tiny bit of leeway, so target is counted as in range.
        private const float IN_RANGE_LEEWAY_IN_M = 0.01f;

        public ITarget Target{ set { _highestPriorityTargetConsumer.Target = value; } }

        public ShipMovementDecider(
            IShip ship,
            ITargetsFactory targetsFactory,
            ITargetDetector enemyDetector,
            ITargetDetector friendDetector)
        {
            Helper.AssertIsNotNull(ship, targetsFactory, enemyDetector, friendDetector);

            _ship = ship;
            _targetsFactory = targetsFactory;

            _blockingEnemyProvider = SetupBlockingEnemyDetection(enemyDetector);
            _blockingFriendlyProvider = SetupBlockingFriendDetection(friendDetector);

            // High priority target provider, for detecting targets that are attacking
            // us but are currently out of range.
            _highPriorityTarget = SetupHighestPriorityTargetProvider();
			_highestPriorityTargetProvider = _highPriorityTarget;
            _highestPriorityTargetConsumer = _highPriorityTarget;

            UpdateVelocity();
        }

        private IBroadCastingTargetProvider SetupBlockingEnemyDetection(ITargetDetector enemyDetector)
        {
            IBroadCastingTargetProvider blockingEnemyProvider = _targetsFactory.CreateShipBlockingEnemyProvider(enemyDetector, _ship);
            blockingEnemyProvider.TargetChanged += OnTargetChanged;
            return blockingEnemyProvider;
        }

        private IBroadCastingTargetProvider SetupBlockingFriendDetection(ITargetDetector friendDetector)
        {
            IBroadCastingTargetProvider blockingFriendlyProvider = _targetsFactory.CreateShipBlockingFriendlyProvider(friendDetector, _ship);
            blockingFriendlyProvider.TargetChanged += OnTargetChanged;
            return blockingFriendlyProvider;
        }

        private IHighestPriorityTargetProvider SetupHighestPriorityTargetProvider()
        {
            ITargetRanker shipTargetRanker = _targetsFactory.CreateShipTargetRanker();
            IHighestPriorityTargetProvider highPriorityTarget = _targetsFactory.CreateHighestPriorityTargetProvider(shipTargetRanker, _ship);

            highPriorityTarget.TargetChanged += OnTargetChanged;
            highPriorityTarget.NewInRangeTarget += OnTargetChanged;

            return highPriorityTarget;
        }

        private void OnTargetChanged(object sender, EventArgs args)
        {
            UpdateVelocity();
        }

        private void UpdateVelocity()
        {
            if (!_ship.IsMoving)
            {
                if (_blockingEnemyProvider.Target == null
                    && _blockingFriendlyProvider.Target == null
                    && (_highestPriorityTargetProvider.Target == null
                        || !IsHighestPriorityTargetWithinRange()))
                {
                    _ship.StartMoving();
                }
            }
            else if (_blockingEnemyProvider.Target != null
                || _blockingFriendlyProvider.Target != null
                || (_highestPriorityTargetProvider.Target != null
                    && IsHighestPriorityTargetWithinRange()))
            {
                _ship.StopMoving();
            }
        }

        // FELIX  This logic should be in it's own class
        private bool IsHighestPriorityTargetWithinRange()
        {
            Assert.IsTrue(_highestPriorityTargetProvider.Target != null);
            float distanceCenterToCenter = Vector2.Distance(_highestPriorityTargetProvider.Target.Position, _ship.Position);
            float distanceCenterToEdge = distanceCenterToCenter - _highestPriorityTargetProvider.Target.Size.x / 2;
            float adjustedDistanceToTarget = distanceCenterToEdge - IN_RANGE_LEEWAY_IN_M;

            Logging.Log(Tags.SHIPS, "Distance: " + adjustedDistanceToTarget + "  Range: " + _ship.OptimalArmamentRangeInM);

            return adjustedDistanceToTarget <= _ship.OptimalArmamentRangeInM;
        }

        public void DisposeManagedState()
        {
            _blockingEnemyProvider.TargetChanged -= OnTargetChanged;
            _blockingFriendlyProvider.TargetChanged -= OnTargetChanged;
            _highPriorityTarget.TargetChanged -= OnTargetChanged;
            _highPriorityTarget.NewInRangeTarget -= OnTargetChanged;
        }
    }
}
