using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    public class PvPRocketBarrelController : PvPBarrelController
    {
        private ICircularList<PvPRocketSpawner> _rocketSpawners;
        private PvPRocketSpawner _middleSpawner;
        private IPvPCruisingProjectileStats _rocketStats;

        public override Vector3 ProjectileSpawnerPosition
        {
            get
            {
                if (_middleSpawner != null) return _middleSpawner.transform.position;
                return Vector3.zero;
            }
        }

        public override bool CanFireWithoutTarget => false;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            PvPRocketSpawner[] rocketSpawners = gameObject.GetComponentsInChildren<PvPRocketSpawner>();
            Assert.IsTrue(rocketSpawners.Length != 0);
            _rocketSpawners = new CircularList<PvPRocketSpawner>(rocketSpawners);

            _middleSpawner = rocketSpawners.Middle();
        }

        protected override IProjectileStats GetProjectileStats()
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
            if (IsServer)
                MuzzleFlashEffectClientRpc();
        }

        public override void CleanUp()
        {
            base.CleanUp();
        }

        [ClientRpc]
        void MuzzleFlashEffectClientRpc()
        {
            _muzzleFlash.Play();
        }
    }
}
