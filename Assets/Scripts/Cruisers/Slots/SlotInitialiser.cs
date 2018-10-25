using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BattleCruisers.Cruisers.Slots
{
    // FELIX   Copy tests from SlotWrapper :)
    public class SlotInitialiser : ISlotInitialiser
    {
        private const int DEFAULT_NUM_OF_NEIGHBOURS = 2;

        public IDictionary<SlotType, ReadOnlyCollection<ISlot>> InitialiseSlots(ICruiser parentCruiser, IList<ISlot> slots, IBuildingPlacer buildingPlacer)
        {
            Helper.AssertIsNotNull(parentCruiser, slots);

            // Sort slots by position (cruiser front to cruiser rear)
            slots
                = slots
                    .OrderBy(slot => slot.Index)
                    .ToList();

            // Initialise slots
            for (int i = 0; i < slots.Count; ++i)
            {
                ISlot slot = slots[i];
                ReadOnlyCollection<ISlot> neighbouringSlots = FindSlotNeighbours(slots, i);
                slot.Initialise(parentCruiser, neighbouringSlots, buildingPlacer);
                slot.IsVisible = false;
            }

            return CreateSlotsMap(slots);
        }

        private ReadOnlyCollection<ISlot> FindSlotNeighbours(IList<ISlot> slots, int slotIndex)
        {
            List<ISlot> neighbouringSlots = new List<ISlot>(DEFAULT_NUM_OF_NEIGHBOURS);

            // Add slot to the front
            if (slotIndex != 0)
            {
                neighbouringSlots.Add(slots[slotIndex - 1]);
            }

            // Add slot to the rear
            if (slotIndex != slots.Count - 1)
            {
                neighbouringSlots.Add(slots[slotIndex + 1]);
            }

            return neighbouringSlots.AsReadOnly();
        }

        private IDictionary<SlotType, ReadOnlyCollection<ISlot>> CreateSlotsMap(IList<ISlot> slots)
        {
            IDictionary<SlotType, ReadOnlyCollection<ISlot>> typeToSlots = new Dictionary<SlotType, ReadOnlyCollection<ISlot>>();

            foreach (SlotType slotType in (SlotType[])Enum.GetValues(typeof(SlotType)))
            {
                ReadOnlyCollection<ISlot> slotsOfType
                    = slots
                        .Where(slot => slot.Type == slotType)
                        .ToList()
                        .AsReadOnly();
                typeToSlots.Add(slotType, slotsOfType);
            }

            return typeToSlots;
        }
    }
}