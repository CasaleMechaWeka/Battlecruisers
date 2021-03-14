using BattleCruisers.Data.Static;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class SmartMissileBarrelController : BarrelController
    {
        private ISmartProjectileStats _smartProjectileStats;
        private SmartMissileSpawner _missileSpawner;
        private ITargetFilter _targetFilter;

        public override Vector3 ProjectileSpawnerPosition => _missileSpawner.transform.position;
        public override bool CanFireWithoutTarget => true;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            _missileSpawner = GetComponentInChildren<SmartMissileSpawner>();
            Assert.IsNotNull(_missileSpawner);
        }

        protected override IProjectileStats GetProjectileStats()
        {
            _smartProjectileStats = GetComponent<ISmartProjectileStats>();
            Assert.IsNotNull(_smartProjectileStats);
            return _smartProjectileStats;
        }

        protected override async Task InternalInitialiseAsync(IBarrelControllerArgs args)
        {
            _targetFilter = new FactionAndTargetTypeFilter(args.EnemyCruiser.Faction, _smartProjectileStats.AttackCapabilities);
            IProjectileSpawnerArgs spawnerArgs
                = new ProjectileSpawnerArgs(
                    args,
                    _smartProjectileStats,
                    TurretStats.BurstSize);

            await _missileSpawner.InitialiseAsync(spawnerArgs, SoundKeys.Firing.Missile, _smartProjectileStats);
        }

        public override void Fire(float angleInDegrees)
        {
            _missileSpawner.SpawnMissile(angleInDegrees: 90, IsSourceMirrored, _targetFilter);
        }
    }
}