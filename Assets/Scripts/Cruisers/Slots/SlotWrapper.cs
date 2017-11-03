using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables.Buildings;
using UnityEngine;

namespace BattleCruisers.Cruisers.Slots
{
    public class SlotWrapper : MonoBehaviour, ISlotWrapper
	{
		private IDictionary<SlotType, List<ISlot>> _slots;
		private SlotType? _highlightedSlotType;

        private const int DEFAULT_NUM_OF_NEIGHBOURS = 2;

        public void Initialise(ICruiser parentCruiser)
		{
            SetupSlots(parentCruiser);
			HideAllSlots();
		}

		private void SetupSlots(ICruiser parentCruiser)
		{
			_slots = new Dictionary<SlotType, List<ISlot>>();

            List<Slot> slots = GetComponentsInChildren<Slot>(includeInactive: true).ToList();

			// Sort slots by position (cruiser front to cruiser rear)
			slots.Sort((slot1, slot2) => slot1.Index.CompareTo(slot2.Index));

            for (int i = 0; i < slots.Count; ++i)
            {
                Slot slot = slots[i];
                IList<ISlot> neighbouringSlots = FindSlotNeighbours(slots, i);
                slot.Initialise(parentCruiser, neighbouringSlots);
                SortSlotsByType(slot);
            }
        }

        private IList<ISlot> FindSlotNeighbours(IList<Slot> slots, int slotIndex)
        {
			IList<ISlot> neighbouringSlots = new List<ISlot>(DEFAULT_NUM_OF_NEIGHBOURS);
			
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

            return neighbouringSlots;
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
            gameObject.SetActive(true);
		}

		public void HideAllSlots()
		{
            gameObject.SetActive(false);
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
					if (slot.IsFree)
					{
						slot.IsActive = true;
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
		}

		private void UnhighlightSlots(SlotType slotType)
		{
			foreach (ISlot slot in _slots[slotType])
			{
				slot.IsActive = false;
			}
		}

		public ISlot GetFreeSlot(SlotType slotType, bool preferFromFront = true)
		{
            return preferFromFront ?
                _slots[slotType].Find(slot => slot.IsFree) :
                _slots[slotType].FindLast(slot => slot.IsFree);
		}

		public int GetSlotCount(SlotType slotType)
		{
			return _slots[slotType].Count;
		}

        public ISlot GetSlot(IBuilding building)
        {
            return _slots[building.SlotType].FirstOrDefault(slot => ReferenceEquals(slot.Building, building));
        }
    }
}
