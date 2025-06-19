using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Slots
{
    public class SlotWrapperController : MonoBehaviour, ISlotNumProvider
    {
        private IList<Slot> _slots;

        // For out of battle scene use
        public void StaticInitialise()
        {
            _slots = GetComponentsInChildren<Slot>(includeInactive: true).ToList();
        }

        // For in battle scene use
        public SlotAccessor Initialise(ICruiser parentCruiser)
        {
            Assert.IsNotNull(parentCruiser);

            SlotInitialiser slotInitialiser = new SlotInitialiser();
            IDictionary<SlotType, ReadOnlyCollection<Slot>> typeToSlots = slotInitialiser.InitialiseSlots(parentCruiser, _slots);

            return new SlotAccessor(typeToSlots);
        }

        public int GetSlotCount(SlotType type)
        {
            return _slots.Count(slot => slot.Type == type);
        }
    }
}
