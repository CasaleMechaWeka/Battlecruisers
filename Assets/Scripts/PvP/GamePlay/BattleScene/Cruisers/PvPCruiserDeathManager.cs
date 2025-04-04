using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public class PvPCruiserDeathManager
    {
        private IPoolable<Vector3> enemyDeathExplosion;
        private Vector3 enemyDeathPosition;
        public PvPCruiserDeathManager(IPvPCruiser playerCruiser, IPvPCruiser enemyCruiser)
        {
            PvPHelper.AssertIsNotNull(playerCruiser, enemyCruiser);

            SetupCruiserDeath(playerCruiser);
            SetupCruiserDeath(enemyCruiser, true);
        }

        private void SetupCruiserDeath(IPvPCruiser cruiser, bool isEnemy = false)
        {
            if (isEnemy)
            {
                PvPCruiserDeathExplosion cruiserDeath = Object.Instantiate(cruiser.DeathPrefab);
                cruiserDeath.transform.rotation = cruiser.Transform.Rotation;
                cruiserDeath.ApplyBodykitWreck(SynchedServerData.Instance.playerBBodykit.Value);
                IPoolable<Vector3> deathExplosion = cruiserDeath.Initialise(PvPFactoryProvider.SettingsManager);
                enemyDeathExplosion = deathExplosion;
                enemyDeathPosition = cruiser.Transform.Position;
                cruiser.Destroyed += (sender, e) => deathExplosion.Activate(cruiser.Transform.Position);
            }
            else
            {
                PvPCruiserDeathExplosion cruiserDeath = Object.Instantiate(cruiser.DeathPrefab);
                cruiserDeath.transform.rotation = cruiser.Transform.Rotation;
                cruiserDeath.ApplyBodykitWreck(SynchedServerData.Instance.playerABodykit.Value);
                IPoolable<Vector3> deathExplosion = cruiserDeath.Initialise(PvPFactoryProvider.SettingsManager);

                cruiser.Destroyed += (sender, e) => deathExplosion.Activate(cruiser.Transform.Position);
            }
        }

        public void ShowDisconnectedCruiserExplosion()
        {
            enemyDeathExplosion.Activate(enemyDeathPosition);
        }
    }
}