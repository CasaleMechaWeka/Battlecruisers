using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Slots
{
    // FELIX  Delete :)
    public class SlotWrapper : ISlotWrapper
	{
		private readonly IDictionary<SlotType, IList<ISlot>> _slots;
        private readonly ISlotFilter _highlightableFilter;
		private SlotType? _highlightedSlotType;

		private const int DEFAULT_NUM_OF_NEIGHBOURS = 2;

		private ISlot _highlightedSlot;
        private ISlot HighlightedSlot
        {
            get { return _highlightedSlot; }
            set
            {
                if (_highlightedSlot != null)
                {
                    _highlightedSlot.IsVisible = false;
                }

                _highlightedSlot = value;

                if (_highlightedSlot != null)
                {
                    _highlightedSlot.IsVisible = true;
                }
            }
        }

        public SlotWrapper(ICruiser parentCruiser, IList<ISlot> slots, ISlotFilter highlightableFilter, IBuildingPlacer buildingPlacer)
		{
            Helper.AssertIsNotNull(parentCruiser, slots, highlightableFilter);

            _highlightableFilter = highlightableFilter;
            _slots = new Dictionary<SlotType, IList<ISlot>>();

            // Sort slots by position (cruiser front to cruiser rear)
            slots 
                = slots
                    .OrderBy(slot => slot.Index)
                    .ToList();

            for (int i = 0; i < slots.Count; ++i)
            {
                ISlot slot = slots[i];
                ReadOnlyCollection<ISlot> neighbouringSlots = FindSlotNeighbours(slots, i);
                slot.Initialise(parentCruiser, neighbouringSlots, buildingPlacer);
                SortSlotsByType(slot);
                slot.IsVisible = false;
            }
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

        private void SortSlotsByType(ISlot slot)
        {
			if (!_slots.ContainsKey(slot.Type))
			{
				_slots[slot.Type] = new List<ISlot>();
			}
			
			_slots[slot.Type].Add(slot);
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

		// Only highlight one slot type at a time
		public void HighlightAvailableSlots(SlotType slotType)
		{
			if (_highlightedSlotType != slotType)
			{
				UnhighlightSlots();

				_highlightedSlotType = slotType;

				foreach (ISlot slot in _slots[slotType])
				{
                    if (_highlightableFilter.IsMatch(slot))
					{
                        slot.IsVisible = true;
					}
				}
			}
		}

		public void UnhighlightSlots()
		{
			if (_highlightedSlotType != null)
			{
				UnhighlightSlots((SlotType)_highlightedSlotType);
				_highlightedSlotType = null;
			}

            HighlightedSlot = null;
		}

		private void UnhighlightSlots(SlotType slotType)
		{
			foreach (ISlot slot in _slots[slotType])
			{
                slot.IsVisible = false;
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
		
        public void HighlightBuildingSlot(IBuilding building)
        {
            HighlightedSlot = GetSlot(building);
            Assert.IsNotNull(HighlightedSlot);
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
    }
}
