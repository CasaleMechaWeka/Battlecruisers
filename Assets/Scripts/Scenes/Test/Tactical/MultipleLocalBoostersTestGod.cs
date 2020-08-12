using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Tactical
{
    public class MultipleLocalBoostersTestGod : TestGodBase
    {
        public Cruiser cruiser;
        public List<Slot> boosterSlots;
        public List<Slot> aaSlots;
        public Slot droneStationSlot;
        public Slot controlTowerSlot;

        protected override List<GameObject> GetGameObjects()
        {
            BCUtils.Helper.AssertIsNotNull(cruiser, droneStationSlot, controlTowerSlot);
            Assert.IsTrue(boosterSlots.Count != 0);
            Assert.IsTrue(aaSlots.Count != 0);

            return new List<GameObject>()
            {
                cruiser.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            helper.SetupCruiser(cruiser);

            IBuildableWrapper<IBuilding> booster = helper.PrefabFactory.GetBuildingWrapperPrefab(StaticPrefabKeys.Buildings.LocalBooster);
            foreach (Slot slot in boosterSlots)
            {
                cruiser.ConstructBuilding(booster, slot);
            }

            IBuildableWrapper<IBuilding> airTurret = helper.PrefabFactory.GetBuildingWrapperPrefab(StaticPrefabKeys.Buildings.AntiAirTurret);
            foreach (Slot slot in aaSlots)
            {
                cruiser.ConstructBuilding(airTurret, slot);
            }

            IBuildableWrapper<IBuilding> navalFactory = helper.PrefabFactory.GetBuildingWrapperPrefab(StaticPrefabKeys.Buildings.NavalFactory);
            ISlot bowSlot = cruiser.SlotAccessor.GetFreeSlot(navalFactory.Buildable.SlotSpecification);
            cruiser.ConstructBuilding(navalFactory, bowSlot);

            IBuildableWrapper<IBuilding> droneStation = helper.PrefabFactory.GetBuildingWrapperPrefab(StaticPrefabKeys.Buildings.DroneStation);
            cruiser.ConstructBuilding(droneStation, droneStationSlot);

            IBuildableWrapper<IBuilding> controlTower = helper.PrefabFactory.GetBuildingWrapperPrefab(StaticPrefabKeys.Buildings.ControlTower);
            cruiser.ConstructBuilding(controlTower, controlTowerSlot);
        }
    }
}