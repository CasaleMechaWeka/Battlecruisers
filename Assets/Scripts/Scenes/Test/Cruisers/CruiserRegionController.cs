using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Cruisers
{
    public class CruiserRegionController : MonoBehaviour
    {
        public Cruiser cruiser;
        public ICruiser Cruiser => cruiser;

        public void StaticInitialise()
        {
            Assert.IsNotNull(cruiser);
        }

        public void Initialise(Helper helper, IList<BCUtils.PrefabKeyName> buildingKeyNames)
        {
            BCUtils.Helper.AssertIsNotNull(helper, buildingKeyNames);

            helper.SetupCruiser(cruiser);

            foreach (BCUtils.PrefabKeyName buildingKeyName in buildingKeyNames)
            {
                BuildingKey buildingKey = BCUtils.StaticPrefabKeyHelper.GetPrefabKey<BuildingKey>(buildingKeyName);
                IBuildableWrapper<IBuilding> building = helper.PrefabFactory.GetBuildingWrapperPrefab(buildingKey);

                IList<ISlot> freeSlots = cruiser.SlotAccessor.GetFreeSlots(building.Buildable.SlotSpecification.SlotType);
                foreach (ISlot slot in freeSlots)
                {
                    cruiser.ConstructBuilding(building, slot);
                }
            }
        }
    }
}