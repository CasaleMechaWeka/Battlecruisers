using BattleCruisers.Buildings;
using BattleCruisers.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Cruisers
{
	public enum SlotType
	{
		SternTop, SternBottom, BowTop, BowBottom, Platform, Deck, Utility, Mast
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
		public UIManager uiManager;

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
			Debug.Log($"Slot.OnMouseDown()  _isActive: {_isActive}");

			if (_isActive)
			{
				Building buildingToBuild = uiManager.SelectedBuilding;

				if (buildingToBuild == null || buildingToBuild.slotType != type)
				{
					throw new InvalidProgramException();
				}

				_building = Instantiate<Building>(buildingToBuild);

				float heightChange = (_renderer.bounds.size.y + _building.Size.y) / 2;
				Vector3 spawnPosition = transform.position + (transform.up * heightChange);

				_building.transform.position = spawnPosition;
				_building.transform.rotation = transform.rotation;

				uiManager.ShowBuildingGroups();

				_building.OnDestroyed = OnBuildingDestroyed;
				_building.UIManager = uiManager;
			}
		}

		private void OnBuildingDestroyed()
		{
			_building = null;
		}
	}
}
