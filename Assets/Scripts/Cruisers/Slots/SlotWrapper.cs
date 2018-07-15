using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Slots
{
    public class SlotWrapper : ISlotWrapper
	{
		private readonly IDictionary<SlotType, IList<ISlot>> _slots;
        private readonly ISlotFilter _highlightableFilter;
		private SlotType? _highlightedSlotType;
        private bool _areMultipleSlotsVisible;

		private const int DEFAULT_NUM_OF_NEIGHBOURS = 2;

		private ISlot _highlightedSlot;
        private ISlot HighlightedSlot
        {
            get { return _highlightedSlot; }
            set
            {
                if (_highlightedSlot != null)
                {
                    _highlightedSlot.UnhighlightSlot();

                    if (!_areMultipleSlotsVisible)
                    {
                        _highlightedSlot.IsVisible = false;
					}
                }

                _highlightedSlot = value;

                if (_highlightedSlot != null)
                {
                    _highlightedSlot.HighlightSlot();
                    _highlightedSlot.IsVisible = true;
                }
            }
        }

        public SlotWrapper(ICruiser parentCruiser, IList<ISlot> slots, ISlotFilter highlightableFilter, IBuildingPlacer buildingPlacer)
		{
            Helper.AssertIsNotNull(parentCruiser, slots, highlightableFilter);

            _highlightableFilter = highlightableFilter;
            _areMultipleSlotsVisible = false;
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
                slot.BuildingDestroyed += Slot_BuildingDestroyed;
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

		public bool IsSlotAvailable(SlotType slotType)
		{
			return _slots[slotType].Any(slot => slot.IsFree);
		}

		public void ShowAllSlots()
		{
            SetSlotVisibility(isVisible: true);
            _areMultipleSlotsVisible = true;
		}

		public void HideAllSlots()
		{
            SetSlotVisibility(isVisible: false);
            HighlightedSlot = null;
            _areMultipleSlotsVisible = false;
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
                        slot.HighlightSlot();
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
                slot.UnhighlightSlot();
			}
		}

		public ISlot GetFreeSlot(SlotType slotType, bool preferFromFront = true)
		{
            return preferFromFront ?
                _slots[slotType].First(slot => slot.IsFree) :
                _slots[slotType].Last(slot => slot.IsFree);
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
            return _slots[building.SlotType].FirstOrDefault(slot => ReferenceEquals(slot.Building, building));
        }

        private void Slot_BuildingDestroyed(object sender, SlotBuildingDestroyedEventArgs e)
        {
            if (_areMultipleSlotsVisible)
            {
				e.BuildingParent.IsVisible = true;

                if (_highlightedSlotType != null && _highlightedSlotType == e.BuildingParent.Type)
                {
                    e.BuildingParent.HighlightSlot();
                }
                else
                {
                    e.BuildingParent.UnhighlightSlot();
                }
            }
            else
            {
                e.BuildingParent.UnhighlightSlot();
                e.BuildingParent.IsVisible = false;
            }
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
