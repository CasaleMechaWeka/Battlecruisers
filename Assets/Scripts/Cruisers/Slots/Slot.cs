using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using BattleCruisers.Cruisers.Slots.Feedback;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Utils;
using System;
using System.Collections.ObjectModel;
using UnityCommon.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.Cruisers.Slots
{
    public class Slot : MonoBehaviour, ISlot, IPointerClickHandler
    {
        private ICruiser _parentCruiser;
        private SpriteRenderer _renderer;
        private IBuildingPlacer _buildingPlacer;
        private Vector2 _size;
        // Hold reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private SlotBoostTextFeedback _boostFeedback;
#pragma warning restore CS0414  // Variable is assigned but never used

        public SlotType type;
        public SlotType Type => type;

        public BuildingFunction buildingFunctionAffinity;
        public BuildingFunction BuildingFunctionAffinity => buildingFunctionAffinity;

        public Direction direction;
        public Direction Direction => direction;

        public float index;
        public float Index => index;

        public bool IsFree => Building == null;
        public ObservableCollection<IBoostProvider> BoostProviders { get; private set; }
        public ReadOnlyCollection<ISlot> NeighbouringSlots { get; private set; }
        public ITransform Transform { get; private set; }
        public Vector3 BuildingPlacementPoint { get; private set; }
        public Vector2 Position => transform.position;

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

            SpriteRenderer slotRenderer = transform.FindNamedComponent<SpriteRenderer>("SlotImage");
            _size = slotRenderer.bounds.size;

            BoostProviders = new ObservableCollection<IBoostProvider>();

            SlotBoostFeedbackInitialiser feedbackInitialiser = GetComponentInChildren<SlotBoostFeedbackInitialiser>();
            Assert.IsNotNull(feedbackInitialiser);
            _boostFeedback = feedbackInitialiser.CreateSlotBoostFeedback(this);

            Transform = new TransformBC(transform);
        }

		public void OnPointerClick(PointerEventData eventData)
		{
            Logging.LogMethod(Tags.SLOTS);

            if (IsVisible && IsFree)
            {
                _parentCruiser.ConstructSelectedBuilding(this);
            }

            Clicked?.Invoke(this, EventArgs.Empty);
		}

		private void OnBuildingDestroyed(object sender, EventArgs e)
		{
			Building = null;

            BuildingDestroyed?.Invoke(this, new SlotBuildingDestroyedEventArgs(this));
		}

        public HighlightArgs CreateHighlightArgs(IHighlightArgsFactory highlightArgsFactory)
        {
            return highlightArgsFactory.CreateForInGameObject(Transform.Position, _size);
        }

        public void SetBuilding(IBuilding building)
        {
            Assert.IsNotNull(building);
            Building = building;
        }
    }
}
