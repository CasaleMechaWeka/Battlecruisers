using BattleCruisers.Buildings;
using BattleCruisers.Units;
using BattleCruisers.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Cruisers
{
	public enum SlotType
	{
		None, SternTop, SternBottom, BowTop, BowBottom, Platform, Deck, Utility, Mast
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
		public Cruiser parentCruiser;
		public BuildingFactory buildingFactory;
		public Direction direction;

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

				_building = buildingFactory.CreateBuilding(buildingToBuild);

				_building.transform.position = FindSpawnPosition();
				_building.transform.rotation = transform.rotation;

				uiManager.ShowBuildingGroups();

				_building.OnDestroyed = OnBuildingDestroyed;
			}
		}

		private Vector3 FindSpawnPosition()
		{
			switch (direction)
			{
				case Direction.Right:
					float horizontalChange = (_renderer.bounds.size.x + _building.Size.x) / 2 + (_building.customOffsetProportion * _building.Size.x);
					return transform.position + (transform.right * horizontalChange);

				case Direction.Up:
					float verticalChange = (_renderer.bounds.size.y + _building.Size.y) / 2 + (_building.customOffsetProportion * _building.Size.y);
					return transform.position + (transform.up * verticalChange);

				default:
					throw new InvalidProgramException();
			}
		}

		private void OnBuildingDestroyed()
		{
			_building = null;
		}
	}
}
