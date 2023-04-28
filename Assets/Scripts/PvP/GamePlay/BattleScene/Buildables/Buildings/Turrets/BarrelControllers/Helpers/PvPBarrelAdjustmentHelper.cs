using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Predictors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public class PvPBarrelAdjustmentHelper : IPvPBarrelAdjustmentHelper
    {
        private readonly IPvPBarrelController _barrelController;
        private readonly IPvPTargetPositionPredictor _targetPositionPredictor;
        private readonly IPvPTargetPositionValidator _targetPositionValidator;
        private readonly IPvPAngleCalculator _angleCalculator;
        private readonly IPvPRotationMovementController _rotationMovementController;
        private readonly IPvPAngleLimiter _angleLimiter;
        private readonly IPvPAttackablePositionFinder _attackablePositionFinder;

        public PvPBarrelAdjustmentHelper(
            IPvPBarrelController barrelController,
            IPvPTargetPositionPredictor targetPositionPredictor,
            IPvPTargetPositionValidator targetPositionValidator,
            IPvPAngleCalculator angleCalculator,
            IPvPRotationMovementController rotationMovementController,
            IPvPAngleLimiter angleLimiter,
            IPvPAttackablePositionFinder attackablePositionFinder)
        {
            PvPHelper.AssertIsNotNull(barrelController, targetPositionPredictor, targetPositionValidator, angleCalculator, rotationMovementController, angleLimiter, attackablePositionFinder);

            _barrelController = barrelController;
            _targetPositionPredictor = targetPositionPredictor;
            _targetPositionValidator = targetPositionValidator;
            _angleCalculator = angleCalculator;
            _rotationMovementController = rotationMovementController;
            _angleLimiter = angleLimiter;
            _attackablePositionFinder = attackablePositionFinder;
        }

        public PvPBarrelAdjustmentResult AdjustTurretBarrel()
        {
            // Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  Target: {_barrelController.CurrentTarget}  Target.IsDestroyed: {_barrelController.CurrentTarget?.IsDestroyed}  Position: {_barrelController.CurrentTarget?.Position}");

            if (_barrelController.CurrentTarget == null || _barrelController.CurrentTarget.IsDestroyed)
            {
                // Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  No alive target, cannot be on target");
                return new PvPBarrelAdjustmentResult(isOnTarget: false);
            }

            Vector2 targetPositionToAttack = _attackablePositionFinder.FindClosestAttackablePosition(_barrelController.ProjectileSpawnerPosition, _barrelController.CurrentTarget);
            float currentAngleInRadians = _barrelController.BarrelAngleInDegrees * Mathf.Deg2Rad;

            Vector2 predictedTargetPosition
                = _targetPositionPredictor.PredictTargetPosition(
                    _barrelController.ProjectileSpawnerPosition,
                    targetPositionToAttack,
                    _barrelController.CurrentTarget,
                    _barrelController.ProjectileStats.MaxVelocityInMPerS,
                    currentAngleInRadians);

            if (!_targetPositionValidator.IsValid(predictedTargetPosition, _barrelController.ProjectileSpawnerPosition, _barrelController.IsSourceMirrored))
            {
                // Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  Target position is invalid, cannot be on target.  predictedTargetPosition: {predictedTargetPosition}  _barrelController.ProjectileSpawnerPosition: {_barrelController.ProjectileSpawnerPosition}  _barrelController.IsSourceMirrored: {_barrelController.IsSourceMirrored}");
                return new PvPBarrelAdjustmentResult(isOnTarget: false);
            }

            float desiredAngleInDegrees
                = _angleCalculator.FindDesiredAngle(
                    _barrelController.ProjectileSpawnerPosition,
                    predictedTargetPosition,
                    _barrelController.IsSourceMirrored);

            float limitedDesiredAngle = _angleLimiter.LimitAngle(desiredAngleInDegrees);

            bool isOnTarget = _rotationMovementController.IsOnTarget(desiredAngleInDegrees);
            if (isOnTarget)
            {
                // Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  On target!");
                return new PvPBarrelAdjustmentResult(isOnTarget, limitedDesiredAngle, predictedTargetPosition);
            }

            _rotationMovementController.AdjustRotation(limitedDesiredAngle);
            // Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  Not on target, but adjusted barrel rotation :)");
            return new PvPBarrelAdjustmentResult(isOnTarget: false);
        }
    }
}
