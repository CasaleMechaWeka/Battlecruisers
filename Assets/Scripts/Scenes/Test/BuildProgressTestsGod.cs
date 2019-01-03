using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class BuildProgressTestsGod : MonoBehaviour 
	{
        public GameObject dummyEnemyCruiser;

		void Start() 
		{
            Helper helper = new Helper(buildSpeedMultiplier: 5);
			
            ICruiser enemyCruiser = helper.CreateCruiser(dummyEnemyCruiser);

            IBuilding[] buildings = FindObjectsOfType<Building>();
            foreach (IBuilding building in buildings)
            {
                helper.InitialiseBuilding(building, enemyCruiser: enemyCruiser);
                building.CompletedBuildable += (sender, e) => building.InitiateDelete();
                building.StartConstruction();
            }
		}
	}
}
