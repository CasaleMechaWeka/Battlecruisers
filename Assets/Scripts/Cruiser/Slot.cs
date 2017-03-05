using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotType
{
	Stern, Bow, Platform, Deck, Utility, Mast
}

public interface ISlot
{
	bool IsFree { get; }
	IBuilding Building { get; }

	void BuildBuilding(IBuilding building);
}

public class Slot : MonoBehaviour, ISlot
{
	public SlotType type;

	public bool IsFree { get { return Building == null; } }
	public IBuilding Building { get; private set; }

	public void BuildBuilding(IBuilding building)
	{
		throw new System.NotImplementedException();
	}
}
