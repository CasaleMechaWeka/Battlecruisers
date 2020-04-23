using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
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

        // FELIX  Add buildings for all slots :)
        public void Initialise(Helper helper, BuildingWrapper deckSlotBuilding)
        {
            BCUtils.Helper.AssertIsNotNull(helper, deckSlotBuilding);

            helper.SetupCruiser(cruiser);

            IList<ISlot> deckSlots = cruiser.SlotAccessor.GetFreeSlots(deckSlotBuilding.Buildable.SlotSpecification.SlotType);
            foreach (ISlot slot in deckSlots)
            {
                cruiser.ConstructBuilding(deckSlotBuilding, slot);
            }
        }
    }
}