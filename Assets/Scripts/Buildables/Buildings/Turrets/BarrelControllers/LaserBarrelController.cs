using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class LaserBarrelController : BarrelController
	{
        private LaserTurretStats _laserTurretStats;
		private LaserEmitter _laserEmitter;

        // Unity does not support child classes with the same private field
        // names as a parent calss, if extending MonoBehaviour.  Hence, cannot
        // use _damagePerS.
        private float _laserDamagePerS;
        public override float DamagePerS { get { return _laserDamagePerS; } }

		public override void StaticInitialise()
		{
            base.StaticInitialise();

            // Laser emitter
            _laserEmitter = gameObject.GetComponentInChildren<LaserEmitter>();
            Assert.IsNotNull(_laserEmitter);

            // Damage per s
            float cycleLength = _laserTurretStats.DurationInS + 1 / _laserTurretStats.FireRatePerS;
            float cycleDamage = _laserTurretStats.DurationInS * _laserTurretStats.DamagePerS;
            _laserDamagePerS = cycleDamage / cycleLength;
        }

        protected override TurretStats SetupTurretStats()
        {
            _laserTurretStats = gameObject.GetComponent<LaserTurretStats>();
            Assert.IsNotNull(_laserTurretStats);
            _laserTurretStats.Initialise();
            return _laserTurretStats;
        }

		protected override IFireIntervalManager SetupFireIntervalManager(TurretStats turretStats)
        {
			LaserFireIntervalManager fireIntervalManager = gameObject.GetComponent<LaserFireIntervalManager>();
			Assert.IsNotNull(fireIntervalManager);
			
            IDurationProvider waitingDurationProvider = _laserTurretStats;
			IDurationProvider firingDurationProvider = new DummyDurationProvider(_laserTurretStats.laserDurationInS);
			fireIntervalManager.Initialise(waitingDurationProvider, firingDurationProvider);
			return fireIntervalManager;
        }
		
		public override void Initialise(
            ITargetFilter targetFilter, 
            IAngleCalculator angleCalculator, 
            IRotationMovementController rotationMovementController,
            IFactoryProvider factoryProvider)
		{
            base.Initialise(targetFilter, angleCalculator, rotationMovementController, factoryProvider);
            _laserEmitter.Initialise(targetFilter, _laserTurretStats.damagePerS);
		}

		protected override void Fire(float angleInDegrees)
		{
			_laserEmitter.FireLaser(angleInDegrees, transform.IsMirrored());
		}

		protected override void CeaseFire()
		{
			_laserEmitter.StopLaser();
		}
	}
}

