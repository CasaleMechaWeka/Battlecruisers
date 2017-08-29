using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    public class KamikazeSignal : Building
    {
        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            GameObject[] aircraftGameObjects = GameObject.FindGameObjectsWithTag(GameObjectTags.AIRCRAFT);

            foreach (GameObject aircraftGameObject in aircraftGameObjects)
            {
                AircraftController aircraftController = aircraftGameObject.GetComponentInChildren<AircraftController>();
                Assert.IsNotNull(aircraftController);

                if (aircraftController.BuildableState == BuildableState.Completed)
                {
                    aircraftController.Kamikaze(_enemyCruiser);
				}
            }
        }
    }
}
