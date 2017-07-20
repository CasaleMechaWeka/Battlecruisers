using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class LaserBarrelController : BarrelController
	{
		private LaserEmitter _laserEmitter;

		public override void StaticInitialise()
		{
			// Turret stats
			LaserTurretStats laserTurretStats = gameObject.GetComponent<LaserTurretStats>();
			Assert.IsNotNull(laserTurretStats);
			laserTurretStats.Initialise();
			TurretStats = laserTurretStats;

			// Fire interval manager
			FireIntervalManager fireIntervalManager = gameObject.GetComponent<FireIntervalManager>();
            Assert.IsNotNull(fireIntervalManager);
            IFireIntervalProvider waitingDurationProvider = laserTurretStats;
            IFireIntervalProvider firingDurationProvider = new DummyDurationProvider(laserTurretStats.laserDurationInS);
            fireIntervalManager.Initialise(waitingDurationProvider, firingDurationProvider);
			_fireIntervalManager = fireIntervalManager;

			// Laser emitter
			_laserEmitter = gameObject.GetComponentInChildren<LaserEmitter>();
			Assert.IsNotNull(_laserEmitter);
		}
		
		public override void Initialise(ITargetFilter targetFilter, IAngleCalculator angleCalculator, IRotationMovementController rotationMovementController)
		{
			base.Initialise(targetFilter, angleCalculator, rotationMovementController);
			_laserEmitter.Initialise(targetFilter, TurretStats.DamagePerS);
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

