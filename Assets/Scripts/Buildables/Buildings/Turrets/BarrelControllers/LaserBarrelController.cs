using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Effects.Laser;
using BattleCruisers.Projectiles.Spawners.Beams.Laser;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class LaserBarrelController : BarrelController
    {
        private LaserTurretStats _laserTurretStats;
        private LaserEmitter _laserEmitter;
        private IManagedDisposable _laserCooldownEffect;

        public override Vector3 ProjectileSpawnerPosition => _laserEmitter.transform.position;
        public override bool CanFireWithoutTarget => false;

        public override void StaticInitialise()
        {
            _baseTurretStats = _laserTurretStats;
            base.StaticInitialise();

            _laserEmitter = GetComponentInChildren<LaserEmitter>();
            Assert.IsNotNull(_laserEmitter);
        }

        protected override TurretStats SetupTurretStats()
        {
            _laserTurretStats = gameObject.GetComponent<LaserTurretStats>();
            Assert.IsNotNull(_laserTurretStats);

            _laserTurretStats.Initialise();
            return _laserTurretStats;
        }

        protected override FireIntervalManager SetupFireIntervalManager(ITurretStats turretStats)
        {
            IDurationProvider waitingDurationProvider = TurretStats;
            IDurationProvider firingDurationProvider = new DummyDurationProvider(_laserTurretStats.laserDurationInS);

            WaitingState waitingState = new WaitingState();
            FiringDurationState firingState = new FiringDurationState();
            
            waitingState.Initialise(firingState, waitingDurationProvider);
            firingState.Initialise(waitingState, firingDurationProvider);
            
            return new FireIntervalManager(firingState);
        }

        protected override IDamageCapability FindDamageCapabilities()
        {
            // Damage per s
            float cycleLength = _laserTurretStats.laserDurationInS + 1 / _laserTurretStats.FireRatePerS;
            float cycleDamage = _laserTurretStats.laserDurationInS * _laserTurretStats.DamagePerS;
            float damagePerS = cycleDamage / cycleLength;

            return new DamageCapability(damagePerS, TurretStats.AttackCapabilities);
        }

        protected override async Task InternalInitialiseAsync(BarrelControllerArgs args)
        {
            _laserEmitter.Initialise(
                args.TargetFilter,
                _laserTurretStats.damagePerS,
                args.Parent,
                args.Updater,
                FactoryProvider.DeferrerProvider.Deferrer);

            ILaserCooldownEffectInitialiser laserCooldownEffectInitialiser = GetComponent<ILaserCooldownEffectInitialiser>();
            Assert.IsNotNull(laserCooldownEffectInitialiser);
            _laserCooldownEffect = laserCooldownEffectInitialiser.CreateLaserCooldownEffect(_laserEmitter);
            await Task.CompletedTask;
        }

        public override void Fire(float angleInDegrees)
        {
            _laserEmitter.FireBeam(angleInDegrees, transform.IsMirrored());
        }

        protected override void CeaseFire()
        {
            _laserEmitter.StopLaser();
        }

        public override void CleanUp()
        {
            base.CleanUp();

            CeaseFire();
            _laserEmitter.DisposeManagedState();
        }
    }
}

