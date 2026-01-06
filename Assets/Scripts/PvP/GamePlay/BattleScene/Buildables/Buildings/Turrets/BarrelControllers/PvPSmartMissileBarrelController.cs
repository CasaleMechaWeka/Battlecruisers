using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    public class PvPSmartMissileBarrelController : PvPBarrelController
    {
        private ProjectileStats _ProjectileStats;
        private PvPSmartMissileSpawner _missileSpawner;
        private new ITargetFilter _targetFilter;

        public override Vector3 ProjectileSpawnerPosition => _missileSpawner.transform.position;
        public override bool CanFireWithoutTarget => true;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            _missileSpawner = GetComponentInChildren<PvPSmartMissileSpawner>();
            Assert.IsNotNull(_missileSpawner);
        }

        protected override ProjectileStats GetProjectileStats()
        {
            _ProjectileStats = GetComponent<ProjectileStats>();
            Assert.IsNotNull(_ProjectileStats);
            return _ProjectileStats;
        }

        protected override async Task InternalInitialiseAsync(PvPBarrelControllerArgs args)
        {
            _targetFilter = new FactionAndTargetTypeFilter(args.EnemyCruiser.Faction, _ProjectileStats.AttackCapabilities);
            PvPProjectileSpawnerArgs spawnerArgs
                = new PvPProjectileSpawnerArgs(
                    args,
                    _ProjectileStats,
                    TurretStats.BurstSize);

            await _missileSpawner.InitialiseAsync(spawnerArgs, SoundKeys.Firing.Missile, _ProjectileStats);
        }

        public override void Fire(float angleInDegrees)
        {
            _missileSpawner.SpawnMissile(angleInDegrees: 90, IsSourceMirrored, _targetFilter);
        }
    }
}