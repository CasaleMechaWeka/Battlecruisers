using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class BuildProgressTestsGod : MonoBehaviour 
	{
        public GameObject dummyEnemyCruiser;

		async void Start() 
		{
            Building[] buildings = FindObjectsOfType<Building>();
            SetActiveness(buildings, false);

            Helper helper = await HelperFactory.CreateHelperAsync(buildSpeedMultiplier: 5);
			
            ICruiser enemyCruiser = helper.CreateCruiser(dummyEnemyCruiser);

            foreach (Building building in buildings)
            {
                building.GameObject.SetActive(true);
                helper.InitialiseBuilding(building, enemyCruiser: enemyCruiser);
                building.CompletedBuildable += (sender, e) => building.InitiateDelete();
                building.StartConstruction();
            }
		}

        private void SetActiveness(Building[] buildings, bool isActive)
        {
            foreach (Building building in buildings)
            {
                building.GameObject.SetActive(isActive);
            }
        }
	}
}
