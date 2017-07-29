using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Cruisers
{
    public class SlotWrapper : MonoBehaviour, ISlotWrapper
	{
		private IDictionary<SlotType, IList<Slot>> _slots;
		private SlotType? _highlightedSlotType;

		public void StaticInitialise()
		{
			SetupSlots();
			HideAllSlots();
		}

		private void SetupSlots()
		{
			_slots = new Dictionary<SlotType, IList<Slot>>();

			Slot[] slots = GetComponentsInChildren<Slot>(includeInactive: true);

			foreach (Slot slot in slots)
			{
				slot.StaticInitialise();

				if (!_slots.ContainsKey(slot.type))
				{
					_slots[slot.type] = new List<Slot>();
				}

				_slots[slot.type].Add(slot);
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

				foreach (Slot slot in _slots[slotType])
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
			foreach (Slot slot in _slots[slotType])
			{
				slot.IsActive = false;
			}
		}

		public ISlot GetFreeSlot(SlotType slotType)
		{
			return _slots[slotType].FirstOrDefault(slot => slot.IsFree);
		}

		public int GetSlotCount(SlotType slotType)
		{
			return _slots[slotType].Count;
		}
	}
}
