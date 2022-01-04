using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using BattleCruisers.Cruisers.Slots.Feedback;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using System.Collections.ObjectModel;
using BattleCruisers.Utils.Properties;
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
        private SlotBoostFeedbackMonitor _boostFeedbackMonitor;
#pragma warning restore CS0414  // Variable is assigned but never used

        public SlotType type;
        public SlotType Type => type;

        public BuildingFunction buildingFunctionAffinity;
        public BuildingFunction BuildingFunctionAffinity => buildingFunctionAffinity;

        public Direction direction;
        public Direction Direction => direction;

        public float index;
        public float Index => index;

        public bool IsFree => _baseBuilding.Value == null;
        public ObservableCollection<IBoostProvider> BoostProviders { get; private set; }
        public ReadOnlyCollection<ISlot> NeighbouringSlots { get; private set; }
        public ITransform Transform { get; private set; }
        public Vector3 BuildingPlacementPoint { get; private set; }
        public Vector2 Position => transform.position;
        
        private ISettableBroadcastingProperty<IBuilding> _baseBuilding;
        public IBroadcastingProperty<IBuilding> Building { get; private set; }

        /// <summary>
        /// Only show/hide slot sprite renderer.  Always show boost feedback.
        /// </summary>
        public bool IsVisible
        {
            get { return _renderer.gameObject.activeSelf; }
            set { _renderer.gameObject.SetActive(value); }
        }

        private IBuilding SlotBuilding
        {
            set
            {
                if (_baseBuilding.Value != null)
                {
                    Assert.IsNull(value);
                    _baseBuilding.Value.Destroyed -= OnBuildingDestroyed;
                }

                _baseBuilding.Value = value;

                if (_baseBuilding.Value != null)
                {
                    _buildingPlacer.PlaceBuilding(_baseBuilding.Value, this);
                    _baseBuilding.Value.Destroyed += OnBuildingDestroyed;
				}
            }
        }

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

            Transform = new TransformBC(transform);

            _baseBuilding = new SettableBroadcastingProperty<IBuilding>(initialValue: null);
            Building = new BroadcastingProperty<IBuilding>(_baseBuilding);

            SlotBoostFeedbackMonitorInitialiser boostFeedbackInitialiser = GetComponentInChildren<SlotBoostFeedbackMonitorInitialiser>();
            Assert.IsNotNull(boostFeedbackInitialiser);
            _boostFeedbackMonitor = boostFeedbackInitialiser.CreateFeedbackMonitor(this);
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
            Invoke("NullifySlotBuilding", 2f);
		}

        private void NullifySlotBuilding()
        {
            SlotBuilding = null;
            IsVisible = true;
        }

        public HighlightArgs CreateHighlightArgs(IHighlightArgsFactory highlightArgsFactory)
        {
            return highlightArgsFactory.CreateForInGameObject(Transform.Position, _size);
        }

        public void SetBuilding(IBuilding building)
        {
            Assert.IsNotNull(building);
            SlotBuilding = building;
        }
    }
}
