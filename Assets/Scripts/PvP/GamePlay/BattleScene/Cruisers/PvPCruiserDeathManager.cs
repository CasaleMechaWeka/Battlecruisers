using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public class PvPCruiserDeathManager
    {
        private IPvPExplosion enemyDeathExplosion;
        private Vector3 enemyDeathPosition;
        public PvPCruiserDeathManager(IPvPCruiser playerCruiser, IPvPCruiser enemyCruiser)
        {
            PvPHelper.AssertIsNotNull(playerCruiser, enemyCruiser);

            SetupCruiserDeath(playerCruiser);
            SetupCruiserDeath(enemyCruiser, true);
        }

        private void SetupCruiserDeath(IPvPCruiser cruiser, bool isEnemy = false)
        {
            if(isEnemy)
            {
                PvPCruiserDeathExplosion cruiserDeath = Object.Instantiate(cruiser.DeathPrefab);
                cruiserDeath.transform.rotation = cruiser.Transform.Rotation;
                IPvPExplosion deathExplosion = cruiserDeath.Initialise(cruiser.FactoryProvider.SettingsManager);
                enemyDeathExplosion = deathExplosion;
                enemyDeathPosition = cruiser.Transform.Position;
                cruiser.Destroyed += (sender, e) => deathExplosion.Activate(cruiser.Transform.Position);
            }
            else
            {
                PvPCruiserDeathExplosion cruiserDeath = Object.Instantiate(cruiser.DeathPrefab);
                cruiserDeath.transform.rotation = cruiser.Transform.Rotation;
                IPvPExplosion deathExplosion = cruiserDeath.Initialise(cruiser.FactoryProvider.SettingsManager);

                cruiser.Destroyed += (sender, e) => deathExplosion.Activate(cruiser.Transform.Position);
            }
        }

        public void ShowDisconnectedCruiserExplosion()
        {
            enemyDeathExplosion.Activate(enemyDeathPosition);
        }
    }
}