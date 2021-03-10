using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public class BarrelFiringHelperTests
    {
        private IBarrelFiringHelper _helper;

        private IBarrelController _barrelController;
        private IAccuracyAdjuster _accuracyAdjuster;
        private IFireIntervalManager _fireIntervalManager;
        private IBarrelFirer _barrelFirer;

        private BarrelAdjustmentResult _onTargetResult, _notOnTargetResult;
        private ITurretStats _turretStats;
        private ITarget _target;

        [SetUp]
        public void TestSetup()
        {
            _barrelController = Substitute.For<IBarrelController>();
            _accuracyAdjuster = Substitute.For<IAccuracyAdjuster>();
            _fireIntervalManager = Substitute.For<IFireIntervalManager>();
            _barrelFirer = Substitute.For<IBarrelFirer>();

            _helper = new BarrelFiringHelper(_barrelController, _accuracyAdjuster, _fireIntervalManager, _barrelFirer);

            _onTargetResult
                = new BarrelAdjustmentResult(
                    isOnTarget: true,
                    desiredAngleInDegrees: 71.17f,
                    predictedTargetPosition: new Vector2(32, 23));
            _notOnTargetResult = new BarrelAdjustmentResult(isOnTarget: false);

            _turretStats = Substitute.For<ITurretStats>();
            _barrelController.TurretStats.Returns(_turretStats);

            _target = Substitute.For<ITarget>();

            Vector3 projectileSpawnerPosition = new Vector3(17, 71, 171);
            _barrelController.ProjectileSpawnerPosition.Returns(projectileSpawnerPosition);
        }

        [Test]
        public void TryFire_FireIntervalManager_ShouldNotFire_DoesNotFire()
        {
            _fireIntervalManager.ShouldFire.Value.Returns(false);

            Assert.IsFalse(_helper.TryFire(_onTargetResult));
            bool compilerBribe = _fireIntervalManager.ShouldFire.Received().Value;
            Expect_NoFire();
        }

        [Test]
        public void TryFire_FireIntervalManager_ShouldFire_NotInBurst_NotOnTarget_DoesNotFire()
        {
            _fireIntervalManager.ShouldFire.Value.Returns(true);
            _turretStats.IsInBurst.Returns(false);

            Assert.IsFalse(_helper.TryFire(_notOnTargetResult));
            Expect_NoFire();
        }

        [Test]
        public void TryFire_FireIntervalManager_ShouldFire_InBurst_NoTarget_CannotFireWithoutTarget_NotOnTarget_DoesNotFire()
        {
            _fireIntervalManager.ShouldFire.Value.Returns(true);
            _turretStats.IsInBurst.Returns(true);
            _barrelController.CurrentTarget.Returns((ITarget)null);
            _barrelController.CanFireWithoutTarget.Returns(false);

            Assert.IsFalse(_helper.TryFire(_notOnTargetResult));
            Expect_NoFire();
        }

        [Test]
        public void TryFire_FireIntervalManager_ShouldFire_InBurst_HasTarget_Fires()
        {
            _fireIntervalManager.ShouldFire.Value.Returns(true);
            _turretStats.IsInBurst.Returns(true);
            _barrelController.CurrentTarget.Returns(_target);

            float barrelAngleInDegrees = 120;
            _barrelController.BarrelAngleInDegrees.Returns(barrelAngleInDegrees);

            float fireAngleInDegrees = 135;

            _accuracyAdjuster.
                FindAngleInDegrees(
                    barrelAngleInDegrees,
                    _barrelController.ProjectileSpawnerPosition,
                    _notOnTargetResult.PredictedTargetPosition,
                    _barrelController.IsSourceMirrored)
                .Returns(fireAngleInDegrees);

            Assert.IsTrue(_helper.TryFire(_notOnTargetResult));
            Expect_Fire(fireAngleInDegrees);
        }

        [Test]
        public void TryFire_FireIntervalManager_ShouldFire_InBurst_NoTarget_CanFireWithoutTarget_Fires()
        {
            _fireIntervalManager.ShouldFire.Value.Returns(true);
            _turretStats.IsInBurst.Returns(true);
            _barrelController.CurrentTarget.Returns((ITarget)null);
            _barrelController.CanFireWithoutTarget.Returns(true);

            float barrelAngleInDegrees = 120;
            _barrelController.BarrelAngleInDegrees.Returns(barrelAngleInDegrees);

            float fireAngleInDegrees = 145;

            _accuracyAdjuster.
                FindAngleInDegrees(
                    barrelAngleInDegrees,
                    _barrelController.ProjectileSpawnerPosition,
                    _notOnTargetResult.PredictedTargetPosition,
                    _barrelController.IsSourceMirrored)
                .Returns(fireAngleInDegrees);

            Assert.IsTrue(_helper.TryFire(_notOnTargetResult));
            Expect_Fire(fireAngleInDegrees);
        }

        [Test]
        public void TryFire_FireIntervalManager_ShouldFire_NotInBurst_OnTarget_Fires()
        {
            _fireIntervalManager.ShouldFire.Value.Returns(true);
            _turretStats.IsInBurst.Returns(false);

            _barrelController.IsSourceMirrored.Returns(true);

            float fireAngleInDegrees = 240;

            _accuracyAdjuster.
                FindAngleInDegrees(
                    _onTargetResult.DesiredAngleInDegrees,
                    _barrelController.ProjectileSpawnerPosition,
                    _onTargetResult.PredictedTargetPosition,
                    _barrelController.IsSourceMirrored)
                .Returns(fireAngleInDegrees);

            Assert.IsTrue(_helper.TryFire(_onTargetResult));
            Expect_Fire(fireAngleInDegrees);
        }

        private void Expect_NoFire()
        {
            _barrelFirer.DidNotReceiveWithAnyArgs().Fire(default);
            _fireIntervalManager.DidNotReceive().OnFired();
        }

        private void Expect_Fire(float fireAngle)
        {
            _barrelFirer.Received().Fire(fireAngle);
            _fireIntervalManager.Received().OnFired();
        }
    }
}