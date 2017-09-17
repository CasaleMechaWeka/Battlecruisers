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

		public override void StaticInitialise()
		{
            _laserTurretStats = gameObject.GetComponent<LaserTurretStats>();
			Assert.IsNotNull(_laserTurretStats);

            base.StaticInitialise();

			// Laser emitter
			_laserEmitter = gameObject.GetComponentInChildren<LaserEmitter>();
			Assert.IsNotNull(_laserEmitter);
		}

		protected override TurretStats SetupTurretStats()
        {
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

