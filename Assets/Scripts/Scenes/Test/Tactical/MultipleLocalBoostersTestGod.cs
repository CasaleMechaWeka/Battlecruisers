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

            IBuildableWrapper<IBuilding> booster = helper.PrefabFactory.GetBuildingWrapperPrefab(StaticPrefabKeys.Buildings.LocalBooster);
            ISlot slot = cruiser.SlotAccessor.GetFreeSlot(booster.Buildable.SlotSpecification);
            cruiser.ConstructBuilding(booster, slot);
        }
    }
}