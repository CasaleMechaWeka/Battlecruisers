using BattleCruisers.Buildables;
using BattleCruisers.Movement.Deciders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Utils;
using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Deciders
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
    public class PvPShipMovementDecider : IMovementDecider
    {
        private readonly PvPShipController _ship;
        private readonly BroadcastingTargetProvider _blockingEnemyProvider, _blockingFriendlyProvider;
        private readonly TargetTracker _inRangeTargetTracker, _shipBlockerTargetTracker;

        private ITarget _highestPriorityTarget;

        private const float IN_RANGE_LEEWAY_IN_M = 0.2f;

        public ITarget Target
        {
            set
            {
                Logging.Log(Tags.SHIP_MOVEMENT_DECIDER, $"_highestPriorityTarget: {_highestPriorityTarget}");
                _highestPriorityTarget = value;
                DecideMovement();
            }
        }

        public PvPShipMovementDecider(
            PvPShipController ship,
            BroadcastingTargetProvider blockingEnemyProvider,
            BroadcastingTargetProvider blockingFriendlyProvider,
            TargetTracker inRangeTargetTracker,
            TargetTracker shipBlockerTargetTracker)
        {
            Helper.AssertIsNotNull(ship, blockingEnemyProvider, blockingFriendlyProvider, inRangeTargetTracker, shipBlockerTargetTracker);

            _ship = ship;
            _blockingEnemyProvider = blockingEnemyProvider;
            _blockingFriendlyProvider = blockingFriendlyProvider;
            _inRangeTargetTracker = inRangeTargetTracker;
            _shipBlockerTargetTracker = shipBlockerTargetTracker;

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
                + $"  target:  {_highestPriorityTarget}"
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
                _highestPriorityTarget != null && IsTargetInRange(_highestPriorityTarget);
        }

        private bool IsTargetInRange(ITarget target)
        {
            float distanceCenterToCenter = Vector2.Distance(target.Position, _ship.Position);
            float distanceCenterToEdge = distanceCenterToCenter - target.Size.x / 2;
            float adjustedDistanceToTarget = distanceCenterToEdge - IN_RANGE_LEEWAY_IN_M;

            // Logging.Log(Tags.TARGET_RANGE_HELPER, $"Target: {target}  Distance: {adjustedDistanceToTarget}  Range: {_ship.OptimalArmamentRangeInM}");

            return adjustedDistanceToTarget <= _ship.OptimalArmamentRangeInM;
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
