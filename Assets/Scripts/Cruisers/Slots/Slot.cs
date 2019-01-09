using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
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
        private CircleCollider2D _collider;
        private IBuildingPlacer _buildingPlacer;
        // Hold reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private SlotBoostFeedback _boostFeedback;
#pragma warning restore CS0414  // Variable is assigned but never used

        public SlotType type;
        public SlotType Type { get { return type; } }

        public BuildingFunction buildingFunctionAffinity;
        public BuildingFunction BuildingFunctionAffinity { get { return buildingFunctionAffinity; } }

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
        public ITransform Transform { get; private set; }
        public virtual Vector2 PositionAdjustment { get { return Vector2.zero; } }
        public Vector2 Size { get { return new Vector2(_collider.radius * 2, _collider.radius * 2); } }
        public virtual float SizeMultiplier { get { return 2; } }
        public HighlightableType HighlightableType { get { return HighlightableType.InGame; } }
        public Vector3 BuildingPlacementPoint { get; private set; }
        public Vector2 Position { get { return transform.position; } }

        public event EventHandler<SlotBuildingDestroyedEventArgs> BuildingDestroyed;
        public event EventHandler Clicked;

        public void Initialise(ICruiser parentCruiser, ReadOnlyCollection<ISlot> neighbouringSlots, IBuildingPlacer buildingPlacer)
		{
            Helper.AssertIsNotNull(parentCruiser, neighbouringSlots, buildingPlacer);

            _parentCruiser = parentCruiser;
            NeighbouringSlots = neighbouringSlots;
            _buildingPlacer = buildingPlacer;

			_renderer = transform.FindNamedComponent<SpriteRenderer>("SlotImage");

            Transform buildingPlacementPoint = transform.FindNamedComponent<Transform>("BuildingPlacementPoint");
            BuildingPlacementPoint = buildingPlacementPoint.position;

            _collider = GetComponent<CircleCollider2D>();
            Assert.IsNotNull(_collider);

            BoostProviders = new ObservableCollection<IBoostProvider>();

            SlotBoostFeedbackInitialiser feedbackInitialiser = GetComponentInChildren<SlotBoostFeedbackInitialiser>();
            Assert.IsNotNull(feedbackInitialiser);
            _boostFeedback = feedbackInitialiser.CreateSlotBoostFeedback(this);

            Transform = new TransformBC(transform);
        }

		public void OnPointerClick(PointerEventData eventData)
		{
            Logging.Log(Tags.SLOTS, "OnPointerClick()");

            if (IsVisible && IsFree)
            {
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

        public HighlightArgs CreateHighlightArgs(IHighlightArgsFactory highlightArgsFactory)
        {
            return highlightArgsFactory.CreateForInGameObject(Transform.Position, Size);
        }
    }
}
