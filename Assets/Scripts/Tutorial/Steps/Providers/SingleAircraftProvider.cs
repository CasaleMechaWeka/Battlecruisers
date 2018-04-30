using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public class SingleAircraftProvider : IProvider<ITarget>
    {
        public ITarget FindItem()
        {
            GameObject[] aircraftGameObjects = GameObject.FindGameObjectsWithTag(GameObjectTags.AIRCRAFT);
			Assert.IsTrue(aircraftGameObjects.Length == 1, "Assumes there will be exactly one aircraft game object.");

            ITarget aircraft = aircraftGameObjects[0].GetComponentInChildren<ITarget>();
            Assert.IsNotNull(aircraft);

            return aircraft;
        }
    }
}
