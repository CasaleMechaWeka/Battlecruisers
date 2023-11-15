using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.BuildingPlacement;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots
{
    public class PvPSlotInitialiser : IPvPSlotInitialiser
    {
        private const int DEFAULT_NUM_OF_NEIGHBOURS = 2;

        public IDictionary<PvPSlotType, ReadOnlyCollection<PvPSlot>> InitialiseSlots(IPvPCruiser parentCruiser, IList<PvPSlot> slots, IPvPBuildingPlacer buildingPlacer)
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
                IPvPSlot slot = slots[i];
                ReadOnlyCollection<IPvPSlot> neighbouringSlots = FindSlotNeighbours(slots, i);
                slot.Initialise(parentCruiser, neighbouringSlots, buildingPlacer);
                slot.IsVisibleRederer = false;
            }

            return CreateSlotsMap(slots);
        }

        private ReadOnlyCollection<IPvPSlot> FindSlotNeighbours(IList<PvPSlot> slots, int slotIndex)
        {
            List<IPvPSlot> neighbouringSlots = new List<IPvPSlot>(DEFAULT_NUM_OF_NEIGHBOURS);

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

        private IDictionary<PvPSlotType, ReadOnlyCollection<PvPSlot>> CreateSlotsMap(IList<PvPSlot> slots)
        {
            IDictionary<PvPSlotType, ReadOnlyCollection<PvPSlot>> typeToSlots = new Dictionary<PvPSlotType, ReadOnlyCollection<PvPSlot>>();

            foreach (PvPSlotType slotType in (PvPSlotType[])Enum.GetValues(typeof(PvPSlotType)))
            {
                ReadOnlyCollection<PvPSlot> slotsOfType
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