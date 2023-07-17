using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    /// <summary>
    /// Supports mutliple shell spawners (barrels).  The turret stats damage
    /// is spread accross the barrels (ie, if there are 2 barrels, each barrel
    /// receives half the turret stats' damage).
    /// </summary>
    public class PvPShellTurretBarrelController : PvPBarrelController
    {
        private PvPShellSpawner[] _shellSpawners;
        private PvPShellSpawner _middleSpawner;

        protected override int NumOfBarrels => _shellSpawners.Length;

        public override Vector3 ProjectileSpawnerPosition => _middleSpawner.transform.position;
        public override bool CanFireWithoutTarget => true;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            _shellSpawners = gameObject.GetComponentsInChildren<PvPShellSpawner>();
            Assert.IsNotNull(_shellSpawners);
            Assert.IsTrue(_shellSpawners.Length != 0);

            _middleSpawner = _shellSpawners.Middle();
        }

        protected override async Task InternalInitialiseAsync(IPvPBarrelControllerArgs args)
        {
            IPvPProjectileSpawnerArgs spawnerArgs = new PvPProjectileSpawnerArgs(args, _projectileStats, pvpTurretStats.BurstSize);

            foreach (PvPShellSpawner spawner in _shellSpawners)
            {
 
                await spawner.InitialiseAsync(spawnerArgs, args.SpawnerSoundKey, args.TargetFilter);
 
            }

  
        }

        public override void Fire(float angleInDegrees)
        {
            OnFireEffectClientRpc();
            foreach (PvPShellSpawner spawner in _shellSpawners)
            {
                spawner.SpawnShell(angleInDegrees, IsSourceMirrored);
            }
        }

        [ClientRpc]
        private void OnFireEffectClientRpc()
        {
            _barrelAnimation?.Play();
            _muzzleFlash?.Play();   
        }
    }
}

