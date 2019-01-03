using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils.DataStrctures;
using NSubstitute;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Tactical
{
    public class LocalBoosterTestGod : MonoBehaviour
    {
        void Start()
        {
            Helper helper = new Helper();


            // Setup target
            AirFactory target = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(target, Faction.Reds);
            target.StartConstruction();
			
			
			// Setup artillery slot
			Slot slotToBoost = FindObjectOfType<Slot>();
            ICruiser parentCruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);
            ReadOnlyCollection<ISlot> emptyNeighbouringSlots = new ReadOnlyCollection<ISlot>(new List<ISlot>());
            IBuildingPlacer buildingPlacer = new BuildingPlacer();
            slotToBoost.Initialise(parentCruiser, emptyNeighbouringSlots, buildingPlacer);


            // Setup artillery
            IExactMatchTargetFilter targetFilter = new ExactMatchTargetFilter()
            {
                Target = target
            };
            ITargetsFactory targetsFactory = helper.CreateTargetsFactory(target.GameObject, targetFilter);

            TurretController turret = FindObjectOfType<TurretController>();
            helper.InitialiseBuilding(turret, Faction.Blues, targetsFactory: targetsFactory, parentSlot: slotToBoost);
            turret.StartConstruction();


            // Setup local booster
			ISlot localBoosterParentSlot = Substitute.For<ISlot>();

            IObservableCollection<IBoostProvider> boostProviders = new ObservableCollection<IBoostProvider>();
            localBoosterParentSlot.BoostProviders.Returns(boostProviders);

            ReadOnlyCollection<ISlot> neighbouringSlots = new ReadOnlyCollection<ISlot>(new List<ISlot>() { slotToBoost });
			localBoosterParentSlot.NeighbouringSlots.Returns(neighbouringSlots);
   
            LocalBoosterController localBooster = FindObjectOfType<LocalBoosterController>();
            helper.InitialiseBuilding(localBooster, parentCruiser: parentCruiser, parentSlot: localBoosterParentSlot);
            localBooster.StartConstruction();
        }
    }
}
