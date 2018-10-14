using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using System;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.Cruisers.Slots
{
    public class Slot : MonoBehaviour, ISlot, IPointerClickHandler
    {
        private ICruiser _parentCruiser;
        private SpriteRenderer _renderer;
        private BoxCollider2D _collider;
        private IBuildingPlacer _buildingPlacer;
        // Hold reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private SlotBoostFeedback _boostFeedback;
#pragma warning restore CS0414  // Variable is assigned but never used

        public SlotType type;
        public SlotType Type { get { return type; } }

        public Direction direction;
        public Direction Direction { get { return direction; } }

        public float index;
        public float Index { get { return index; } }

        public bool IsFree { get { return Building == null; } }
        public IObservableCollection<IBoostProvider> BoostProviders { get; private set; }
        public ReadOnlyCollection<ISlot> NeighbouringSlots { get; private set; }
		
        /// <summary>
        /// Only show/hide slot sprite renderer.  Always show boost feedback.
        /// </summary>
        public bool IsVisible
        {
            get { return _renderer.gameObject.activeSelf; }
            set { _renderer.gameObject.SetActive(value); }
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
                    _buildingPlacer.PlaceBuilding(_building, this);
                    _building.Destroyed += OnBuildingDestroyed;
				}
            }
        }

        // IHighlightable
        public Transform Transform { get { return transform; } }
        public virtual Vector2 PositionAdjustment { get { return Vector2.zero; } }
        public Vector2 Size { get { return _collider.size; } }
        public virtual float SizeMultiplier { get { return 2; } }
        public HighlightableType HighlightableType { get { return HighlightableType.InGame; } }

        public event EventHandler<SlotBuildingDestroyedEventArgs> BuildingDestroyed;
        public event EventHandler Clicked;

        public void Initialise(ICruiser parentCruiser, ReadOnlyCollection<ISlot> neighbouringSlots, IBuildingPlacer buildingPlacer)
		{
            Helper.AssertIsNotNull(parentCruiser, neighbouringSlots, buildingPlacer);

            _parentCruiser = parentCruiser;
            NeighbouringSlots = neighbouringSlots;
            _buildingPlacer = buildingPlacer;

			_renderer = transform.FindNamedComponent<SpriteRenderer>("SlotImage");
			Assert.IsNotNull(_renderer);

            _collider = GetComponent<BoxCollider2D>();
            Assert.IsNotNull(_collider);

            BoostProviders = new ObservableCollection<IBoostProvider>();

            SlotBoostFeedbackInitialiser feedbackInitialiser = GetComponentInChildren<SlotBoostFeedbackInitialiser>();
            Assert.IsNotNull(feedbackInitialiser);
            _boostFeedback = feedbackInitialiser.CreateSlotBoostFeedback(this);
        }

		public void OnPointerClick(PointerEventData eventData)
		{
            Logging.Log(Tags.SLOTS, "OnPointerClick()");

            // FELIX  Don't want to steal clicks from cruiser...
            if (IsVisible)
            {
                Assert.IsTrue(IsFree);
                _parentCruiser.ConstructSelectedBuilding(this);
            }

            if (Clicked != null)
            {
                Clicked.Invoke(this, EventArgs.Empty);
            }
		}

		private void OnBuildingDestroyed(object sender, EventArgs e)
		{
			Building = null;

            if (BuildingDestroyed != null)
            {
                BuildingDestroyed.Invoke(this, new SlotBuildingDestroyedEventArgs(this));
            }
		}
    }
}
