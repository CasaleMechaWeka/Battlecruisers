using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    public class PvPSamSiteBarrelController : PvPBarrelController
    {
        private IPvPExactMatchTargetFilter _exactMatchTargetFilter;
        private PvPMissileSpawner _missileSpawner;

        public override Vector3 ProjectileSpawnerPosition => _missileSpawner.transform.position;
        public override bool CanFireWithoutTarget => false;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            _missileSpawner = gameObject.GetComponentInChildren<PvPMissileSpawner>();
            Assert.IsNotNull(_missileSpawner);
        }

        public async Task InitialiseAsync(IPvPExactMatchTargetFilter targetFilter, IPvPBarrelControllerArgs args)
        {
            await base.InitialiseAsync(args);

            _exactMatchTargetFilter = targetFilter;
            IPvPProjectileSpawnerArgs spawnerArgs = new PvPProjectileSpawnerArgs(args, _projectileStats, pvpTurretStats.BurstSize);

            await _missileSpawner.InitialiseAsync(spawnerArgs, args.SpawnerSoundKey);
        }

        public override void Fire(float angleInDegrees)
        {
            _exactMatchTargetFilter.Target = Target;
            _missileSpawner.SpawnMissile(angleInDegrees, IsSourceMirrored, Target, _exactMatchTargetFilter);
        }
    }
}
