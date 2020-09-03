using BattleCruisers.Effects.Explosions;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Cruisers
{
    public class CruiserDeathManager
    {
        public CruiserDeathManager(ICruiser playerCruiser, ICruiser aiCruiser)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser);

            SetupCruiserDeath(playerCruiser);
            SetupCruiserDeath(aiCruiser);
        }

        private void SetupCruiserDeath(ICruiser cruiser)
        {
            ExplosionController cruiserDeath = Object.Instantiate(cruiser.DeathPrefab);
            cruiserDeath.transform.rotation = cruiser.Transform.Rotation;
            IExplosion deathExplosion = cruiserDeath.Initialise();

            cruiser.Destroyed += (sender, e) => deathExplosion.Activate(cruiser.Transform.Position);
        }
    }
}