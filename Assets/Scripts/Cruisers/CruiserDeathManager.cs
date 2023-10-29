using BattleCruisers.Data;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Utils;
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
            cruiserDeath.ApplyBodykitWreck(ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.PlayerLoadout.SelectedBodykit);
            IExplosion deathExplosion = cruiserDeath.Initialise(cruiser.FactoryProvider.SettingsManager);
            cruiser.Destroyed += (sender, e) => deathExplosion.Activate(cruiser.Transform.Position);
        }
    }
}