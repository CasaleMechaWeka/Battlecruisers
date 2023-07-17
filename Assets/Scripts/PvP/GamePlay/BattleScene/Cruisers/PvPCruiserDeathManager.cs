using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public class PvPCruiserDeathManager
    {
        public PvPCruiserDeathManager(IPvPCruiser playerCruiser, IPvPCruiser enemyCruiser)
        {
            PvPHelper.AssertIsNotNull(playerCruiser, enemyCruiser);

            SetupCruiserDeath(playerCruiser);
            SetupCruiserDeath(enemyCruiser);
        }

        private void SetupCruiserDeath(IPvPCruiser cruiser)
        {
            PvPCruiserDeathExplosion cruiserDeath = Object.Instantiate(cruiser.DeathPrefab);
            cruiserDeath.transform.rotation = cruiser.Transform.Rotation;
            IPvPExplosion deathExplosion = cruiserDeath.Initialise(cruiser.FactoryProvider.SettingsManager);

            cruiser.Destroyed += (sender, e) => deathExplosion.Activate(cruiser.Transform.Position);
        }
    }
}