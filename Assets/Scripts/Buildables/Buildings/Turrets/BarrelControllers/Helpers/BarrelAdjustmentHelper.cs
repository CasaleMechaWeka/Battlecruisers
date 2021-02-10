using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public class BarrelAdjustmentHelper : IBarrelAdjustmentHelper
    {
        private readonly IBarrelController _barrelController;
        private readonly ITargetPositionPredictor _targetPositionPredictor;
        private readonly ITargetPositionValidator _targetPositionValidator;
        private readonly IAngleCalculator _angleCalculator;
        private readonly IRotationMovementController _rotationMovementController;
        private readonly IAngleLimiter _angleLimiter;
        private readonly IAttackablePositionFinder _attackablePositionFinder;

        public BarrelAdjustmentHelper(
            IBarrelController barrelController,
            ITargetPositionPredictor targetPositionPredictor,
            ITargetPositionValidator targetPositionValidator,
            IAngleCalculator angleCalculator,
            IRotationMovementController rotationMovementController,
            IAngleLimiter angleLimiter,
            IAttackablePositionFinder attackablePositionFinder)
        {
            Helper.AssertIsNotNull(barrelController, targetPositionPredictor, targetPositionValidator, angleCalculator, rotationMovementController, angleLimiter, attackablePositionFinder);

            _barrelController = barrelController;
            _targetPositionPredictor = targetPositionPredictor;
            _targetPositionValidator = targetPositionValidator;
            _angleCalculator = angleCalculator;
            _rotationMovementController = rotationMovementController;
            _angleLimiter = angleLimiter;
            _attackablePositionFinder = attackablePositionFinder;
        }

        public BarrelAdjustmentResult AdjustTurretBarrel()
        {
            if (_barrelController.CurrentTarget == null || _barrelController.CurrentTarget.IsDestroyed)
            {
                Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  No alive target, cannot be on target");
                return new BarrelAdjustmentResult(isOnTarget: false);
            }

            Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  Target: {_barrelController.CurrentTarget}  Position: {_barrelController.CurrentTarget.Position}");

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
                Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  Target position is invalid, cannot be on target.  predictedTargetPosition: {predictedTargetPosition}  _barrelController.ProjectileSpawnerPosition: {_barrelController.ProjectileSpawnerPosition}  _barrelController.IsSourceMirrored: {_barrelController.IsSourceMirrored}");
                return new BarrelAdjustmentResult(isOnTarget: false);
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
                Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  On target!");
                return new BarrelAdjustmentResult(isOnTarget, limitedDesiredAngle, predictedTargetPosition);
            }

            _rotationMovementController.AdjustRotation(limitedDesiredAngle);
            Logging.Verbose(Tags.BARREL_CONTROLLER, $"{_barrelController}  Not on target, but adjusted barrel rotation :)");
            return new BarrelAdjustmentResult(isOnTarget: false);
        }
    }
}
