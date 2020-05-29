using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Tactical
{
    public class MultipleLocalBoostersTestGod : TestGodBase
    {
        public Cruiser cruiser;
        public List<Slot> boosterSlots;

        protected override List<GameObject> GetGameObjects()
        {
            Assert.IsNotNull(cruiser);
            return new List<GameObject>()
            {
                cruiser.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            helper.SetupCruiser(cruiser);

            // FELIX
            //ISlot slot = cruiser.SlotAccessor.GetFreeSlot(booster.Buildable.SlotSpecification);

            IBuildableWrapper<IBuilding> booster = helper.PrefabFactory.GetBuildingWrapperPrefab(StaticPrefabKeys.Buildings.LocalBooster);
            foreach (Slot slot in boosterSlots)
            {
                cruiser.ConstructBuilding(booster, slot);
            }
        }
    }
}