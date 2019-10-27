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
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Tactical
{
    public class LocalBoosterTestGod : TestGodBase
    {
        private AirFactory _target;
        private TurretController _turret;

        protected override IList<GameObject> GetGameObjects()
        {
            _target = FindObjectOfType<AirFactory>();
            _turret = FindObjectOfType<TurretController>();

            return new List<GameObject>()
            {
                _target.GameObject,
                _turret.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            // Setup target
            helper.InitialiseBuilding(_target, Faction.Reds);
            _target.StartConstruction();
			
			
			// Setup artillery slot
			Slot slotToBoost = FindObjectOfType<Slot>();
            ICruiser parentCruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);
            ReadOnlyCollection<ISlot> emptyNeighbouringSlots = new ReadOnlyCollection<ISlot>(new List<ISlot>());
            IBuildingPlacer buildingPlacer 
                = new BuildingPlacer(
                    new BuildingPlacerCalculator());
            slotToBoost.Initialise(parentCruiser, emptyNeighbouringSlots, buildingPlacer);


            // Setup artillery
            IExactMatchTargetFilter targetFilter = new ExactMatchTargetFilter()
            {
                Target = _target
            };
            ITargetFactories targetFactories = helper.CreateTargetFactories(_target.GameObject, targetFilter: targetFilter);

            helper.InitialiseBuilding(_turret, Faction.Blues, targetFactories: targetFactories, parentSlot: slotToBoost);
            _turret.StartConstruction();


            // Setup local booster
			ISlot localBoosterParentSlot = Substitute.For<ISlot>();

            ObservableCollection<IBoostProvider> boostProviders = new ObservableCollection<IBoostProvider>();
            localBoosterParentSlot.BoostProviders.Returns(boostProviders);

            ReadOnlyCollection<ISlot> neighbouringSlots = new ReadOnlyCollection<ISlot>(new List<ISlot>() { slotToBoost });
			localBoosterParentSlot.NeighbouringSlots.Returns(neighbouringSlots);
   
            LocalBoosterController localBooster = FindObjectOfType<LocalBoosterController>();
            helper.InitialiseBuilding(localBooster, parentCruiser: parentCruiser, parentSlot: localBoosterParentSlot);
            localBooster.StartConstruction();
        }
    }
}
