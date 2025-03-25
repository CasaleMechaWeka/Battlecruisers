using BattleCruisers.Data;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Cruisers
{
    public class CruiserDeathManager
    {
        public CruiserDeathManager(Cruiser playerCruiser, Cruiser aiCruiser)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser);

            SetupCruiserDeath(playerCruiser);
            SetupCruiserDeath(aiCruiser);
        }

        private void SetupCruiserDeath(Cruiser cruiser)
        {
            CruiserDeathExplosion cruiserDeath = Object.Instantiate(cruiser.DeathPrefab);
            cruiserDeath.transform.rotation = cruiser.Transform.Rotation;
            cruiserDeath.ApplyBodykitWreck(DataProvider.GameModel.PlayerLoadout.SelectedBodykit);
            IPoolable<Vector3> deathExplosion = cruiserDeath.Initialise(cruiser.FactoryProvider.SettingsManager);
            cruiser.Destroyed += (sender, e) => deathExplosion.Activate(cruiser.Transform.Position);
        }
    }
}