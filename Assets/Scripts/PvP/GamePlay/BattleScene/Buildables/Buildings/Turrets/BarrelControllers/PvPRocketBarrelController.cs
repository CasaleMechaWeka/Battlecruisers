using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    public class PvPRocketBarrelController : PvPBarrelController
    {
        private IPvPCircularList<PvPRocketSpawner> _rocketSpawners;
        private PvPRocketSpawner _middleSpawner;
        private IPvPCruisingProjectileStats _rocketStats;

        public override Vector3 ProjectileSpawnerPosition => _middleSpawner.transform.position;
        public override bool CanFireWithoutTarget => false;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            PvPRocketSpawner[] rocketSpawners = gameObject.GetComponentsInChildren<PvPRocketSpawner>();
            Assert.IsTrue(rocketSpawners.Length != 0);
            _rocketSpawners = new PvPCircularList<PvPRocketSpawner>(rocketSpawners);

            _middleSpawner = rocketSpawners.Middle();
        }

        protected override IPvPProjectileStats GetProjectileStats()
        {
            _rocketStats = GetComponent<PvPCruisingProjectileStats>();
            Assert.IsNotNull(_rocketStats);
            return _rocketStats;
        }

        protected override async Task InternalInitialiseAsync(IPvPBarrelControllerArgs args)
        {
            IPvPProjectileSpawnerArgs spawnerArgs = new PvPProjectileSpawnerArgs(args, _rocketStats, pvpTurretStats.BurstSize);

            foreach (PvPRocketSpawner rocketSpawner in _rocketSpawners.Items)
            {
                await rocketSpawner.InitialiseAsync(spawnerArgs, args.SpawnerSoundKey, _rocketStats);
            }
        }

        public override void Fire(float angleInDegrees)
        {
            _rocketSpawners
                .Next()
                .SpawnRocket(
                    angleInDegrees,
                    transform.IsMirrored(),
                    Target,
                    _targetFilter);
        }
    }
}
