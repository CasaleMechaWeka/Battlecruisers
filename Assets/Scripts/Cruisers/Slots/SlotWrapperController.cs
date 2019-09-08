using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Slots
{
    public class SlotWrapperController : MonoBehaviour, ISlotNumProvider
    {
        private IList<ISlot> _slots;

        // For out of battle scene use
        public void StaticInitialise()
        {
            _slots = GetComponentsInChildren<ISlot>(includeInactive: true).ToList();
        }

        // For in battle scene use
        public ISlotAccessor Initialise(ICruiser parentCruiser)
        {
            Assert.IsNotNull(parentCruiser);

            IBuildingPlacer buildingPlacer 
                = new BuildingPlacer(
                    new BuildingPlacerCalculator());
            ISlotInitialiser slotInitialiser = new SlotInitialiser();
            IDictionary<SlotType, ReadOnlyCollection<ISlot>> typeToSlots = slotInitialiser.InitialiseSlots(parentCruiser, _slots, buildingPlacer);

            return new SlotAccessor(typeToSlots);
        }

        public int GetSlotCount(SlotType type)
        {
            return _slots.Count(slot => slot.Type == type);
        }
    }
}
