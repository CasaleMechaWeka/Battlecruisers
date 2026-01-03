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
        private ProjectileStats _ProjectileStats;
        private SmartMissileSpawner _missileSpawner;
        private new ITargetFilter _targetFilter;

        public override Vector3 ProjectileSpawnerPosition => _missileSpawner.transform.position;
        public override bool CanFireWithoutTarget => true;
        [SerializeField]
        private float firingAngle = 90f;
        [SerializeField]
        private bool fixFiringAngle = true;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            _missileSpawner = GetComponentInChildren<SmartMissileSpawner>();
            Assert.IsNotNull(_missileSpawner);
        }

        protected override ProjectileStats GetProjectileStats()
        {
            _ProjectileStats = GetComponent<ProjectileStats>();
            Assert.IsNotNull(_ProjectileStats);
            return _ProjectileStats;
        }

        protected override async Task InternalInitialiseAsync(BarrelControllerArgs args)
        {
            _targetFilter = new FactionAndTargetTypeFilter(args.EnemyCruiser.Faction, _ProjectileStats.AttackCapabilities);
            IProjectileSpawnerArgs spawnerArgs
                = new ProjectileSpawnerArgs(
                    args,
                    _ProjectileStats,
                    TurretStats.BurstSize);

            await _missileSpawner.InitialiseAsync(spawnerArgs, SoundKeys.Firing.Missile, _ProjectileStats);
        }

        public override void Fire(float angleInDegrees)
        {
            float angle = fixFiringAngle ? firingAngle : angleInDegrees;
            _missileSpawner.SpawnMissile(angle, IsSourceMirrored, _targetFilter);
            Debug.Log("fired");
        }
    }
}