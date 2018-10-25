using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Slots
{
    // FELIX   Copy tests from SlotWrapper :)
    public class SlotAccessor : ISlotAccessor
    {
		private readonly IDictionary<SlotType, ReadOnlyCollection<ISlot>> _slots;

		private const int DEFAULT_NUM_OF_NEIGHBOURS = 2;

        public SlotAccessor(ICruiser parentCruiser, IList<ISlot> slots, IBuildingPlacer buildingPlacer)
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

            _slots = CreateSlotsMap(slots);
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

		public bool IsSlotAvailable(SlotSpecification slotSpecification)
		{
			return 
                _slots[slotSpecification.SlotType]
                    .Any(slot => FreeSlotFilter(slot, slotSpecification.BuildingFunction));
		}

        private void SetSlotVisibility(bool isVisible)
        {
            foreach (IList<ISlot> slots in _slots.Values)
            {
                foreach (ISlot slot in slots)
                {
                    slot.IsVisible = isVisible;
                }
            }
        }

		public ISlot GetFreeSlot(SlotSpecification slotSpecification)
		{
            return slotSpecification.PreferFromFront ?
                _slots[slotSpecification.SlotType].First(slot => FreeSlotFilter(slot, slotSpecification.BuildingFunction)) :
                _slots[slotSpecification.SlotType].Last(slot => FreeSlotFilter(slot, slotSpecification.BuildingFunction));
		}

        private bool FreeSlotFilter(ISlot slot, BuildingFunction buildingFunction)
        {
            return
                slot.IsFree
                && (buildingFunction == BuildingFunction.Generic
                    || slot.BuildingFunctionAffinity == buildingFunction);
        }

		public int GetSlotCount(SlotType slotType)
		{
			return _slots[slotType].Count;
		}
		
        private ISlot GetSlot(IBuilding building)
        {
            return 
                _slots[building.SlotSpecification.SlotType]
                    .FirstOrDefault(slot => ReferenceEquals(slot.Building, building));
        }

        public ReadOnlyCollection<ISlot> GetFreeSlots(SlotType slotType)
        {
            Assert.IsTrue(_slots.ContainsKey(slotType));

            List<ISlot> freeSlots
                = _slots[slotType]
                    .Where(slot => slot.IsFree)
                    .ToList();

            return freeSlots.AsReadOnly();
        }

        public ReadOnlyCollection<ISlot> GetSlots(SlotType slotType)
        {
            Assert.IsTrue(_slots.ContainsKey(slotType));
            return _slots[slotType];
        }
    }
}
