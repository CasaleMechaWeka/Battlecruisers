using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Cruisers.Slots
{
    public class SlotWrapper : MonoBehaviour, ISlotWrapper
	{
		private IDictionary<SlotType, List<ISlot>> _slots;
		private SlotType? _highlightedSlotType;

        public ReadOnlyCollection<ISlot> Slots { get; private set; }

        public void Initialise(ICruiser parentCruiser)
		{
            SetupSlots(parentCruiser);
			HideAllSlots();
		}

		private void SetupSlots(ICruiser parentCruiser)
		{
			_slots = new Dictionary<SlotType, List<ISlot>>();

			Slot[] slots = GetComponentsInChildren<Slot>(includeInactive: true);
            Slots = new ReadOnlyCollection<ISlot>(slots);

            // Sort slots by type
			foreach (Slot slot in slots)
			{
                slot.Initialise(parentCruiser);

				if (!_slots.ContainsKey(slot.type))
				{
					_slots[slot.type] = new List<ISlot>();
				}

				_slots[slot.type].Add(slot);
			}

            // Sort slots by position (cruiser front to cruiser rear)
            foreach (List<ISlot> slotsOfAType in _slots.Values)
            {
                slotsOfAType.Sort((slot1, slot2) => slot1.XDistanceFromParentCruiser.CompareTo(slot2.XDistanceFromParentCruiser));
            }
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
    }
}
