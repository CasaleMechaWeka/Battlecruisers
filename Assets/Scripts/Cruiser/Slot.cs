using System;
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
	bool IsActive { set; }
}

public class Slot : MonoBehaviour, ISlot
{
	private SpriteRenderer _renderer;
	private Building _building;

	public SlotType type;
	public BuildMenuController buildMenu;

	public bool IsFree { get { return _building == null; } }

	private bool _isActive;
	public bool IsActive
	{
		set
		{
			if (_isActive != value)
			{
				_isActive = value;
				_renderer.color = _isActive ? ACTIVE_COLOUR : DEFAULT_COLOUR;
			}
		}
	}

	public static Color DEFAULT_COLOUR = Color.yellow;
	public static Color ACTIVE_COLOUR = Color.green;

	void Awake()
	{
		_isActive = false;
		_renderer = GetComponent<SpriteRenderer>();
		_renderer.color = DEFAULT_COLOUR;
	}

	void OnMouseDown()
	{
		if (_isActive)
		{
			Building buildingToBuild = buildMenu.SelectedBuilding;

			if (buildingToBuild == null || buildingToBuild.slotType != type)
			{
				throw new InvalidProgramException();
			}

			Vector3 spawnPosition = transform.position + (transform.up * (buildingToBuild.Size.y - _renderer.bounds.size.y));
			_building = Instantiate<Building>(buildingToBuild, spawnPosition, transform.rotation);
			_building.ShowBuilding();

			buildMenu.ShowBuildingGroups();
		}
	}
}
