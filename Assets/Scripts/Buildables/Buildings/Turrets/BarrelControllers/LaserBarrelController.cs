using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Projectiles.Spawners.Laser;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class LaserBarrelController : BarrelController
	{
        private LaserTurretStats _laserTurretStats;
		private LaserEmitter _laserEmitter;

        public override Vector3 ProjectileSpawnerPosition => _laserEmitter.transform.position;
        public override bool CanFireWithoutTarget => false;

        public override void StaticInitialise()
		{
            base.StaticInitialise();

            // Laser emitter
            _laserEmitter = gameObject.GetComponentInChildren<LaserEmitter>();
            Assert.IsNotNull(_laserEmitter);
        }

        protected override TurretStats SetupTurretStats()
        {
            _laserTurretStats = gameObject.GetComponent<LaserTurretStats>();
            Assert.IsNotNull(_laserTurretStats);
            _laserTurretStats.Initialise();
            return _laserTurretStats;
        }

        protected override IFireIntervalManager SetupFireIntervalManager(ITurretStats turretStats)
        {
            LaserFireIntervalManagerInitialiser fireIntervalManagerInitialiser = gameObject.GetComponent<LaserFireIntervalManagerInitialiser>();
            Assert.IsNotNull(fireIntervalManagerInitialiser);
            
            IDurationProvider waitingDurationProvider = _laserTurretStats;
            IDurationProvider firingDurationProvider = new DummyDurationProvider(_laserTurretStats.laserDurationInS);
            return fireIntervalManagerInitialiser.Initialise(waitingDurationProvider, firingDurationProvider);
        }
        
        protected override IDamageCapability FindDamageCapabilities()
        {
			// Damage per s
			float cycleLength = _laserTurretStats.DurationInS + 1 / _laserTurretStats.FireRatePerS;
			float cycleDamage = _laserTurretStats.DurationInS * _laserTurretStats.DamagePerS;
            float damagePerS = cycleDamage / cycleLength;

            return new DamageCapability(damagePerS, TurretStats.AttackCapabilities);
        }

        public override void Initialise(IBarrelControllerArgs args)
		{
            base.Initialise(args);
            _laserEmitter.Initialise(args.TargetFilter, _laserTurretStats.damagePerS, args.Parent, args.FactoryProvider.Sound.SoundFetcher);
		}

        public override void Fire(float angleInDegrees)
		{
			_laserEmitter.FireLaser(angleInDegrees, transform.IsMirrored());
		}

		protected override void CeaseFire()
		{
			_laserEmitter.StopLaser();
		}
	}
}

