using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotStats
{
	public int NumOfSternSlots { get; private set; }
	public int NumOfBowSlots { get; private set; }
	public int NumOfPlatformSlots { get; private set; }
	public int NumOfDeckSlots { get; private set; }
	public int NumOfUtilitySlots { get; private set; }
	public int NumOfMastSlots { get; private set; }
}

public interface ICruiser : IDamagable
{
	float Health { get; }
	SlotStats SlotStats { get; }

	bool IsSlotAvailable(SlotType slotType);
	void ShowAvailableSlots(SlotType slotType);
}

public class TridentCruiser : MonoBehaviour, ICruiser
{
	private IDictionary<SlotType, IList<Slot>> _slots;

	public float Health { get; private set; }

	// FELIX  Unused?
	public SlotStats SlotStats { get; private set; }

	void Start()
	{
		_slots = new Dictionary<SlotType, IList<Slot>>();

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

		// FELIX TEMP
		foreach (SlotType type in _slots.Keys)
		{
			Debug.Log($"{_slots[type].Count} {type} slots");
		}
	}

	public bool IsSlotAvailable(SlotType slotType)
	{
		return true;
	}

	public void ShowAvailableSlots(SlotType slotType)
	{
	}

	public void TakeDamage(float damage)
	{

	}
}
