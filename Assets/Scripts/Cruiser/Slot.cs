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
	Building Building { get; }
	Color Colour { set; }

	void BuildBuilding(Building building);
}

public class Slot : MonoBehaviour, ISlot
{
	private SpriteRenderer _renderer;

	public SlotType type;

	public bool IsFree { get { return Building == null; } }
	public Building Building { get; private set; }

	private Color _colour;
	public Color Colour
	{
		set
		{
			if (value != _colour)
			{
				_colour = value;
				_renderer.color = value;
			}
		}
	}

	public static Color DEFAULT_COLOUR = Color.yellow;
	public static Color ACTIVE_COLOUR = Color.green;

	void Awake()
	{
		_colour = DEFAULT_COLOUR;
		_renderer = GetComponent<SpriteRenderer>();
	}

	public void BuildBuilding(Building building)
	{
		throw new System.NotImplementedException();
	}
}
