using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.Factories;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class BuildProgressTestsGod : MonoBehaviour 
	{
        public GameObject dummyEnemyCruiser;

		void Start() 
		{
            Helper helper = new Helper(buildSpeedMultiplier: 5);
			
            ITargetsFactory targetsFactory = Substitute.For<ITargetsFactory>();
            ICruiser enemyCruiser = helper.CreateCruiser(dummyEnemyCruiser);

            IBuilding[] buildings = FindObjectsOfType<Building>();
            foreach (IBuilding building in buildings)
            {
                helper.InitialiseBuilding(building, targetsFactory: targetsFactory, enemyCruiser: enemyCruiser);
                building.CompletedBuildable += (sender, e) => building.InitiateDelete();
                building.StartConstruction();
            }
		}
	}
}
