using System;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.Cruisers
{
    public enum SlotType
	{
		None, SternTop, SternBottom, BowTop, BowBottom, Platform, Deck, Utility, Mast
	}

    // FELIX  Move to own class
    // FELIX  Create slot manager
	public interface ISlot
	{
		bool IsFree { get; }
		SlotType Type { get; }
		bool IsActive { set; }
		IBuilding Building { set; }
	}

	public class Slot : MonoBehaviour, ISlot, IPointerClickHandler
	{
		private SpriteRenderer _renderer;
		private ICruiser _parentCruiser;

		public SlotType type;
		public Direction direction;

		public bool IsFree { get { return _building == null; } }
		public SlotType Type { get { return type; } }

		private IBuilding _building;
		public IBuilding Building
		{
			set
			{
				Assert.IsNotNull(value);
				Assert.IsNull(_building);

				_building = value;

                _building.Position = FindSpawnPosition(_building);
				_building.Rotation = transform.rotation;
				_building.Destroyed += OnBuildingDestroyed;
			}
		}

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

		public void StaticInitialise()
		{
			_isActive = false;

			_renderer = GetComponent<SpriteRenderer>();
			Assert.IsNotNull(_renderer);
			_renderer.color = DEFAULT_COLOUR;

            _parentCruiser = gameObject.GetComponentInInactiveParent<ICruiser>();
			Assert.IsNotNull(_parentCruiser);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (_isActive)
			{
				_parentCruiser.ConstructSelectedBuilding(this);
			}
		}

		private Vector3 FindSpawnPosition(IBuilding building)
		{
			switch (direction)
			{
				case Direction.Right:
					float horizontalChange = (_renderer.bounds.size.x + building.Size.x) / 2 + (building.CustomOffsetProportion * building.Size.x);
					return transform.position + (transform.right * horizontalChange);

				case Direction.Up:
					float verticalChange = (_renderer.bounds.size.y + building.Size.y) / 2 + (building.CustomOffsetProportion * building.Size.y);
					return transform.position + (transform.up * verticalChange);

				default:
					throw new InvalidProgramException();
			}
		}

		private void OnBuildingDestroyed(object sender, EventArgs e)
		{
			_building.Destroyed -= OnBuildingDestroyed;
			_building = null;
		}
	}
}
