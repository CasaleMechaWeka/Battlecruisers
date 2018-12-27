using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Projectiles.Stats;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public class BarrelAdjustmentHelperTests
    {
        private IBarrelAdjustmentHelper _helper;

        private IBarrelController _barrelController;
        private ITargetPositionPredictor _targetPositionPredictor;
        private ITargetPositionValidator _targetPositionValidator;
        private IAngleCalculator _angleCalculator;
        private IRotationMovementController _rotationMovementController;
        private IAngleLimiter _angleLimiter;
        private IAttackablePositionFinder _attackablePositionFinder;

        private ITarget _target;
        private IProjectileStats _projectileStats;
        private Vector2 _targetPositionToAttack, _predictedTargetPosition;
        private float _desiredAngleInDegrees, _limitedAngelInDegrees;

        [SetUp]
        public void TestSetup()
        {
            _barrelController = Substitute.For<IBarrelController>();
            _targetPositionPredictor = Substitute.For<ITargetPositionPredictor>();
            _targetPositionValidator = Substitute.For<ITargetPositionValidator>();
            _angleCalculator = Substitute.For<IAngleCalculator>();
            _rotationMovementController = Substitute.For<IRotationMovementController>();
            _angleLimiter = Substitute.For<IAngleLimiter>();
            _attackablePositionFinder = Substitute.For<IAttackablePositionFinder>();

            _helper
                = new BarrelAdjustmentHelper(
                    _barrelController,
                    _targetPositionPredictor,
                    _targetPositionValidator,
                    _angleCalculator,
                    _rotationMovementController,
                    _angleLimiter,
                    _attackablePositionFinder);

            // Non-destroyed target check
            _target = Substitute.For<ITarget>();
            _target.IsDestroyed.Returns(false);
            _barrelController.CurrentTarget.Returns(_target);

            // Predicted target position validation
            Vector3 projectileSpawnerPosition = new Vector3(91, 82, 73);
            _barrelController.ProjectileSpawnerPosition.Returns(projectileSpawnerPosition);

            _projectileStats = Substitute.For<IProjectileStats>();
            _projectileStats.MaxVelocityInMPerS.Returns(32.54f);
            _barrelController.ProjectileStats.Returns(_projectileStats);

            _barrelController.BarrelAngleInDegrees.Returns(17);
            float barrelAngleInRadians = _barrelController.BarrelAngleInDegrees * Mathf.Deg2Rad;

            _targetPositionToAttack = new Vector2(27, 83);
            _predictedTargetPosition = new Vector2(87, 56);

            _targetPositionPredictor
                .PredictTargetPosition(
                    projectileSpawnerPosition,
                    _targetPositionToAttack,
                    _target,
                    _projectileStats.MaxVelocityInMPerS,
                    barrelAngleInRadians)
                .Returns(_predictedTargetPosition);

            _barrelController.IsSourceMirrored.Returns(true);

            SetupTargetPositionValidation(isValid: true);

            // Angle calculation and limitation
            _desiredAngleInDegrees = 93.71f;
            _angleCalculator
                .FindDesiredAngle(
                    projectileSpawnerPosition,
                    _predictedTargetPosition,
                    _barrelController.IsSourceMirrored)
                .Returns(_desiredAngleInDegrees);

            _limitedAngelInDegrees = 17.39f;
            _angleLimiter
                .LimitAngle(_desiredAngleInDegrees)
                .Returns(_limitedAngelInDegrees);

            SetupIsOnTarget(isOnTarget: false);

            // Finding attackable position
            _attackablePositionFinder
                .FindClosestAttackablePosition(projectileSpawnerPosition, _target)
                .Returns(_targetPositionToAttack);
        }

        [Test]
        public void AdjustTurretBarrel_TargetIsNull()
        {
            _barrelController.CurrentTarget.Returns((ITarget)null);
            BarrelAdjustmentResult expectedResult = new BarrelAdjustmentResult(isOnTarget: false);
            Assert.AreEqual(expectedResult, _helper.AdjustTurretBarrel());
        }

        [Test]
        public void AdjustTurretBarrel_TargetIsDestroyed()
        {
            _target.IsDestroyed.Returns(true);
            BarrelAdjustmentResult expectedResult = new BarrelAdjustmentResult(isOnTarget: false);
            Assert.AreEqual(expectedResult, _helper.AdjustTurretBarrel());
        }

        [Test]
        public void AdjustTurretBarrel_TargetPositionIsInvalid()
        {
            SetupTargetPositionValidation(isValid: false);
            BarrelAdjustmentResult expectedResult = new BarrelAdjustmentResult(isOnTarget: false);
            Assert.AreEqual(expectedResult, _helper.AdjustTurretBarrel());

            _targetPositionValidator
                .Received()
                .IsValid(
                    _predictedTargetPosition,
                    _barrelController.ProjectileSpawnerPosition,
                    _barrelController.IsSourceMirrored);
        }

        [Test]
        public void AdjustTurretBarrel_BarrelIsOnTarget()
        {
            bool isOnTarget = true;
            SetupIsOnTarget(isOnTarget);

            BarrelAdjustmentResult expectedResult = new BarrelAdjustmentResult(isOnTarget, _limitedAngelInDegrees, _predictedTargetPosition);
            Assert.AreEqual(expectedResult, _helper.AdjustTurretBarrel());

            _rotationMovementController
                .Received()
                .IsOnTarget(_desiredAngleInDegrees);
        }

        [Test]
        public void AdjustTurretBarrel_BarrelIsAdjusted()
        {
            SetupIsOnTarget(isOnTarget: false);

            BarrelAdjustmentResult expectedResult = new BarrelAdjustmentResult(isOnTarget: false);
            Assert.AreEqual(expectedResult, _helper.AdjustTurretBarrel());

            _rotationMovementController.Received().AdjustRotation(_limitedAngelInDegrees);
        }

        private void SetupTargetPositionValidation(bool isValid)
        {
            _targetPositionValidator
                .IsValid(
                    _predictedTargetPosition,
                    _barrelController.ProjectileSpawnerPosition,
                    _barrelController.IsSourceMirrored)
                .Returns(isValid);
        }

        private void SetupIsOnTarget(bool isOnTarget)
        {
            _rotationMovementController
                .IsOnTarget(_desiredAngleInDegrees)
                .Returns(isOnTarget);
        }
    }
}
