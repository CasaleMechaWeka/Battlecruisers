using System;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Slots.States;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.Cruisers.Slots
{
    public class Slot : MonoBehaviourWrapper, ISlot, IPointerClickHandler
    {
        private SpriteRenderer _renderer;
        private ICruiser _parentCruiser;
        private ISlotState _defaultState, _highlightedEmptyState, _highlightedFullState;

        public SlotType type;
        public SlotType Type { get { return type; } }

        public Direction direction;
        public Direction Direction { get { return direction; } }

        public float index;
        public float Index { get { return index; } }

        public bool IsFree { get { return Building == null; } }
        public IObservableCollection<IBoostProvider> BoostProviders { get; private set; }
        public ReadOnlyCollection<ISlot> NeighbouringSlots { get; private set; }
		
        private ISlotState _currentState;
        private ISlotState CurrentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;
                _renderer.color = _currentState.Colour;
            }
        }

        private IBuilding _building;
        public IBuilding Building
        {
            get { return _building; }
            set
            {
                if (_building != null)
                {
                    Assert.IsNull(value);
                    _building.Destroyed -= OnBuildingDestroyed;
                }

                _building = value;

                if (_building != null)
                {
                    _building.Position = FindSpawnPosition(_building);
                    _building.Rotation = FindBuildingRotation();
                    _building.Destroyed += OnBuildingDestroyed;
				}
            }
        }

        // IHighlightable
        public Transform Transform { get { return transform; } }
        public virtual Vector2 PositionAdjustment { get { return Vector2.zero; } }
        public Vector2 Size { get { return _renderer.size; } }
        public virtual float SizeMultiplier { get { return 0.6f; } }
        public HighlightableType HighlightableType { get { return HighlightableType.InGame; } }

        public event EventHandler<SlotBuildingDestroyedEventArgs> BuildingDestroyed;
        public event EventHandler Clicked;

        public void Initialise(ICruiser parentCruiser, ReadOnlyCollection<ISlot> neighbouringSlots)
		{
            Helper.AssertIsNotNull(parentCruiser, neighbouringSlots);

            _parentCruiser = parentCruiser;
            NeighbouringSlots = neighbouringSlots;

			_renderer = GetComponent<SpriteRenderer>();
			Assert.IsNotNull(_renderer);

            BoostProviders = new ObservableCollection<IBoostProvider>();

            _defaultState = new DefaultState();
            _highlightedFullState = new HighlightedFullState();
            _highlightedEmptyState = new HighlightedEmptyState(_parentCruiser, this);

            CurrentState = _defaultState;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
            CurrentState.OnClick();

            if (Clicked != null)
            {
                Clicked.Invoke(this, EventArgs.Empty);
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
                    throw new ArgumentException("Invalid slot direction");
			}
		}

        private Quaternion FindBuildingRotation()
        {
            return transform.rotation;
        }

		private void OnBuildingDestroyed(object sender, EventArgs e)
		{
			Building = null;

            if (BuildingDestroyed != null)
            {
                BuildingDestroyed.Invoke(this, new SlotBuildingDestroyedEventArgs(this));
            }
		}

        public void HighlightSlot()
        {
            CurrentState = IsFree ? _highlightedEmptyState : _highlightedFullState;
        }

        public void UnhighlightSlot()
        {
            CurrentState = _defaultState;
        }
    }
}
