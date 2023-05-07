using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    public class PvPMissileBarrelController : PvPBarrelController
    {
        private IPvPCircularList<PvPMissileSpawner> _missileSpawners;
        private PvPMissileSpawner _middleSpawner;

        public override Vector3 ProjectileSpawnerPosition => _middleSpawner.transform.position;
        public override bool CanFireWithoutTarget => false;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            PvPMissileSpawner[] missileSpawners = gameObject.GetComponentsInChildren<PvPMissileSpawner>();
            Assert.IsTrue(missileSpawners.Length != 0);
            _missileSpawners = new PvPCircularList<PvPMissileSpawner>(missileSpawners);

            _middleSpawner = missileSpawners.Middle();
        }

        protected override async Task InternalInitialiseAsync(IPvPBarrelControllerArgs args)
        {
            IPvPProjectileSpawnerArgs spawnerArgs = new PvPProjectileSpawnerArgs(args, _projectileStats, pvpTurretStats.BurstSize);

            foreach (PvPMissileSpawner missileSpawner in _missileSpawners.Items)
            {
                await missileSpawner.InitialiseAsync(spawnerArgs, args.SpawnerSoundKey);
            }
        }

        public override void Fire(float angleInDegrees)
        {
            // Logging.Log(Tags.BARREL_CONTROLLER, $"{this}  angleInDegrees: " + angleInDegrees);

            _missileSpawners.Next().SpawnMissile(
                angleInDegrees,
                IsSourceMirrored,
                Target,
                _targetFilter);
        }
    }
}
