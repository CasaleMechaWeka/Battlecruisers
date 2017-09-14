using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class BuildProgressTestsGod : MonoBehaviour 
	{
        public GameObject dummyEnemyCruiser;

		void Start() 
		{
            Helper helper = new Helper(numOfDrones: 2);
			
            ITargetsFactory targetsFactory = Substitute.For<ITargetsFactory>();
            ICruiser enemyCruiser = helper.CreateCruiser(dummyEnemyCruiser);
            ISlot parentSlot = Substitute.For<ISlot>();
            ReadOnlyCollection<ISlot> neighbouringSlots = new ReadOnlyCollection<ISlot>(new List<ISlot>());
            parentSlot.NeighbouringSlots.Returns(neighbouringSlots);

            IBuilding[] buildings = FindObjectsOfType<Building>();
            foreach (IBuilding building in buildings)
            {
                helper.InitialiseBuilding(building, targetsFactory: targetsFactory, enemyCruiser: enemyCruiser, parentSlot: parentSlot);
                building.StartConstruction();
            }
		}
	}
}
