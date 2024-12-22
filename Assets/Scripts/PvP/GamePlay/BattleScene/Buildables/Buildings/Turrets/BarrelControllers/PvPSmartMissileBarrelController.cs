using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    public class PvPSmartMissileBarrelController : PvPBarrelController
    {
        private IPvPSmartProjectileStats _smartProjectileStats;
        private PvPSmartMissileSpawner _missileSpawner;
        private IPvPTargetFilter _targetFilter;

        public override Vector3 ProjectileSpawnerPosition => _missileSpawner.transform.position;
        public override bool CanFireWithoutTarget => true;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            _missileSpawner = GetComponentInChildren<PvPSmartMissileSpawner>();
            Assert.IsNotNull(_missileSpawner);
        }

        protected override IPvPProjectileStats GetProjectileStats()
        {
            _smartProjectileStats = GetComponent<IPvPSmartProjectileStats>();
            Assert.IsNotNull(_smartProjectileStats);
            return _smartProjectileStats;
        }

        protected override async Task InternalInitialiseAsync(IPvPBarrelControllerArgs args)
        {
            _targetFilter = new PvPFactionAndTargetTypeFilter(args.EnemyCruiser.Faction, _smartProjectileStats.AttackCapabilities);
            IPvPProjectileSpawnerArgs spawnerArgs
                = new PvPProjectileSpawnerArgs(
                    args,
                    _smartProjectileStats,
                    pvpTurretStats.BurstSize);

            await _missileSpawner.InitialiseAsync(spawnerArgs, PvPSoundKeys.PvPFiring.Missile, _smartProjectileStats);
        }

        public override void Fire(float angleInDegrees)
        {
            _missileSpawner.SpawnMissile(angleInDegrees: 90, IsSourceMirrored, _targetFilter);
        }
    }
}