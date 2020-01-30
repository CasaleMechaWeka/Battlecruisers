using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Effects;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using NUnit.Framework;
using System;
using UnityEngine;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public class BarrelFiringHelperTests
    {
        private IBarrelFiringHelper _helper;

        private IBarrelController _barrelController;
        private IAccuracyAdjuster _accuracyAdjuster;
        private IFireIntervalManager _fireIntervalManager;
        private IAnimation _barrelFiringAnimation;
        private IParticleSystemGroup _muzzleFlash;
        private IConstantDeferrer _deferrer;

        private BarrelAdjustmentResult _onTargetResult, _notOnTargetResult;
        private ITurretStats _turretStats;
        private ITarget _target;

        [SetUp]
        public void TestSetup()
        {
            _barrelController = Substitute.For<IBarrelController>();
            _accuracyAdjuster = Substitute.For<IAccuracyAdjuster>();
            _fireIntervalManager = Substitute.For<IFireIntervalManager>();
            _barrelFiringAnimation = Substitute.For<IAnimation>();
            _muzzleFlash = Substitute.For<IParticleSystemGroup>();
            _deferrer = Substitute.For<IConstantDeferrer>();

            // FELIX  Fix :P
            _helper = new BarrelFiringHelper(_barrelController, _accuracyAdjuster, _fireIntervalManager, null);

            _onTargetResult
                = new BarrelAdjustmentResult(
                    isOnTarget: true,
                    desiredAngleInDegrees: 71.17f,
                    predictedTargetPosition: new Vector2(32, 23));
            _notOnTargetResult = new BarrelAdjustmentResult(isOnTarget: false);

            _turretStats = Substitute.For<ITurretStats>();
            _barrelController.TurretStats.Returns(_turretStats);

            _target = Substitute.For<ITarget>();

            _deferrer.Defer(Arg.Invoke());
        }

        [Test]
        public void TryFire_FireIntervalManager_ShouldNotFire_DoesNotFire()
        {
            _fireIntervalManager.ShouldFire.Returns(false);

            Assert.IsFalse(_helper.TryFire(_onTargetResult));
            bool compilerBribe = _fireIntervalManager.Received().ShouldFire;
            Expect_NoFire();
        }

        [Test]
        public void TryFire_FireIntervalManager_ShouldFire_NotInBurst_NotOnTarget_DoesNotFire()
        {
            _fireIntervalManager.ShouldFire.Returns(true);
            _turretStats.IsInBurst.Returns(false);

            Assert.IsFalse(_helper.TryFire(_notOnTargetResult));
            Expect_NoFire();
        }

        [Test]
        public void TryFire_FireIntervalManager_ShouldFire_InBurst_NoTarget_CannotFireWithoutTarget_NotOnTarget_DoesNotFire()
        {
            _fireIntervalManager.ShouldFire.Returns(true);
            _turretStats.IsInBurst.Returns(true);
            _barrelController.CurrentTarget.Returns((ITarget)null);
            _barrelController.CanFireWithoutTarget.Returns(false);

            Assert.IsFalse(_helper.TryFire(_notOnTargetResult));
            Expect_NoFire();
        }

        [Test]
        public void TryFire_FireIntervalManager_ShouldFire_InBurst_HasTarget_Fires()
        {
            _fireIntervalManager.ShouldFire.Returns(true);
            _turretStats.IsInBurst.Returns(true);
            _barrelController.CurrentTarget.Returns(_target);

            float barrelAngleInDegrees = 120;
            _barrelController.BarrelAngleInDegrees.Returns(barrelAngleInDegrees);

            Assert.IsTrue(_helper.TryFire(_notOnTargetResult));
            Expect_Fire(barrelAngleInDegrees);
        }

        [Test]
        public void TryFire_FireIntervalManager_ShouldFire_InBurst_NoTarget_CanFireWithoutTarget_Fires()
        {
            _fireIntervalManager.ShouldFire.Returns(true);
            _turretStats.IsInBurst.Returns(true);
            _barrelController.CurrentTarget.Returns((ITarget)null);
            _barrelController.CanFireWithoutTarget.Returns(true);

            float barrelAngleInDegrees = 120;
            _barrelController.BarrelAngleInDegrees.Returns(barrelAngleInDegrees);

            Assert.IsTrue(_helper.TryFire(_notOnTargetResult));
            Expect_Fire(barrelAngleInDegrees);
        }

        [Test]
        public void TryFire_FireIntervalManager_ShouldFire_NotInBurst_OnTarget_Fires()
        {
            _fireIntervalManager.ShouldFire.Returns(true);
            _turretStats.IsInBurst.Returns(false);

            Vector3 projectileSpawnerPosition = new Vector3(17, 71, 171);
            _barrelController.ProjectileSpawnerPosition.Returns(projectileSpawnerPosition);

            _barrelController.IsSourceMirrored.Returns(true);

            float fireAngleInDegrees = 240;

            _accuracyAdjuster.
                FindAngleInDegrees(
                _onTargetResult.DesiredAngleInDegrees,
                projectileSpawnerPosition,
                _onTargetResult.PredictedTargetPosition,
                _barrelController.IsSourceMirrored)
                .Returns(fireAngleInDegrees);

            Assert.IsTrue(_helper.TryFire(_onTargetResult));
            Expect_Fire(fireAngleInDegrees);
        }

        private void Expect_NoFire()
        {
            _deferrer.DidNotReceiveWithAnyArgs().Defer(default);
            _barrelController.DidNotReceiveWithAnyArgs().Fire(default);
            _fireIntervalManager.DidNotReceive().OnFired();
            _barrelFiringAnimation.DidNotReceive().Play();
            _muzzleFlash.DidNotReceive().Play();
        }

        private void Expect_Fire(float fireAngle)
        {
            _deferrer.Received().Defer(Arg.Any<Action>());
            _barrelController.Received().Fire(fireAngle);
            _fireIntervalManager.Received().OnFired();
            _barrelFiringAnimation.Received().Play();
            _muzzleFlash.Received().Play();
        }
    }
}