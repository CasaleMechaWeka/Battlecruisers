using BattleCruisers.Buildings;
using BattleCruisers.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Cruisers
{
	public class SlotStats
	{
		public int NumOfSternSlots { get; private set; }
		public int NumOfBowSlots { get; private set; }
		public int NumOfPlatformSlots { get; private set; }
		public int NumOfDeckSlots { get; private set; }
		public int NumOfUtilitySlots { get; private set; }
		public int NumOfMastSlots { get; private set; }
	}

	public interface ICruiser : IDamagable, IRepairable
	{
		bool IsSlotAvailable(SlotType slotType);
		void HighlightAvailableSlots(SlotType slotType);
		void UnhighlightSlots();
	}

	public class Cruiser : MonoBehaviour, ICruiser
	{
		private IDictionary<SlotType, IList<Slot>> _slots;
		private GameObject _slotsWrapper;
		private SlotType? _highlightedSlotType;

		public BuildingLoadout loadout;
		public HealthBarController healthBarController;

		private float _health;
		private float Health
		{ 
			get { return _health; }
			set
			{
				_health = value;
				healthBarController.Health = _health;
			}
		}
		public float startingHealth;

		void Start()
		{
			SetupSlots();
			HideAllSlots();
			healthBarController.Initialise(startingHealth);
			Health = startingHealth;

			// FELIX TEMP
			Health *= 0.8f;
		}

		private void SetupSlots()
		{
			_slots = new Dictionary<SlotType, IList<Slot>>();
			_slotsWrapper = transform.FindChild("SlotsWrapper").gameObject;
			
			Slot[] slots = GetComponentsInChildren<Slot>();
			Debug.Log($"slots.Length: {slots.Length}");
			
			foreach (Slot slot in slots)
			{
				if (!_slots.ContainsKey(slot.type))
				{
					_slots[slot.type] = new List<Slot>();
				}
				
				_slots[slot.type].Add(slot);
			}
		}

		public bool IsSlotAvailable(SlotType slotType)
		{
			return true;
		}

		public void ShowAllSlots()
		{
			_slotsWrapper.SetActive(true);
		}

		public void HideAllSlots()
		{
			_slotsWrapper.SetActive(false);
		}

		// FELIX  Disable clicking on other slots?
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

		public void TakeDamage(float damageAmount)
		{

		}

		public void Repair(float repairAmount)
		{

		}

		void OnMouseDown()
		{
			Debug.Log("Cruiesr.OnMouseDown()");
		}
	}
}
