using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Laser;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams.Laser;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    public class PvPLaserBarrelController : PvPBarrelController
    {
        private PvPLaserTurretStats _laserTurretStats;
        private PvPLaserEmitter _laserEmitter;
        private IPvPManagedDisposable _laserCooldownEffect;

        public override Vector3 ProjectileSpawnerPosition => _laserEmitter.transform.position;
        public override bool CanFireWithoutTarget => false;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            _laserEmitter = GetComponentInChildren<PvPLaserEmitter>();
            Assert.IsNotNull(_laserEmitter);
        }

        protected override PvPTurretStats SetupTurretStats()
        {
            _laserTurretStats = gameObject.GetComponent<PvPLaserTurretStats>();
            Assert.IsNotNull(_laserTurretStats);
            _laserTurretStats.Initialise();
            return _laserTurretStats;
        }

        protected override IPvPFireIntervalManager SetupFireIntervalManager(IPvPTurretStats turretStats)
        {
            PvPLaserFireIntervalManagerInitialiser fireIntervalManagerInitialiser = gameObject.GetComponent<PvPLaserFireIntervalManagerInitialiser>();
            Assert.IsNotNull(fireIntervalManagerInitialiser);

            IPvPDurationProvider waitingDurationProvider = _laserTurretStats;
            IPvPDurationProvider firingDurationProvider = new PvPDummyDurationProvider(_laserTurretStats.laserDurationInS);
            return fireIntervalManagerInitialiser.Initialise(waitingDurationProvider, firingDurationProvider);
        }

        protected override IPvPDamageCapability FindDamageCapabilities()
        {
            // Damage per s
            float cycleLength = _laserTurretStats.DurationInS + 1 / _laserTurretStats.FireRatePerS;
            float cycleDamage = _laserTurretStats.DurationInS * _laserTurretStats.DamagePerS;
            float damagePerS = cycleDamage / cycleLength;

            return new PvPDamageCapability(damagePerS, pvpTurretStats.AttackCapabilities);
        }

        protected override async Task InternalInitialiseAsync(IPvPBarrelControllerArgs args)
        {
            await
                _laserEmitter.InitialiseAsync(
                    args.TargetFilter,
                    _laserTurretStats.damagePerS,
                    args.Parent,
                   /* args.FactoryProvider.SettingsManager,*/ null,
                    args.Updater,
                    args.FactoryProvider.DeferrerProvider.Deferrer);
            IPvPLaserCooldownEffectInitialiser laserCooldownEffectInitialiser = GetComponent<IPvPLaserCooldownEffectInitialiser>();
            Assert.IsNotNull(laserCooldownEffectInitialiser);
            _laserCooldownEffect = laserCooldownEffectInitialiser.CreateLaserCooldownEffect(_laserEmitter);
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

