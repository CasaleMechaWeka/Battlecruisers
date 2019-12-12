using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

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
            GameObject cruiserDeath = Object.Instantiate(cruiser.DeathPrefab);
            cruiserDeath.transform.position = cruiser.Transform.Position;
            cruiserDeath.transform.rotation = cruiser.Transform.Rotation;

            IGameObject cruiserDeathGameObject = cruiserDeath.GetComponent<IGameObject>();
            Assert.IsNotNull(cruiserDeathGameObject);
            cruiserDeathGameObject.IsVisible = false;

            cruiser.Destroyed += (sender, e) => cruiserDeathGameObject.IsVisible = true;
        }
    }
}