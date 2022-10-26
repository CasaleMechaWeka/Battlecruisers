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
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Scenes.BattleScene;

namespace BattleCruisers.Cruisers.Slots
{
    public class Slot : MonoBehaviour, ISlot, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler, IDragHandler
    {
        private ICruiser _parentCruiser;
        private SpriteRenderer _renderer;
        private IExplosion _explosion;
        public ExplosionController _explosionController;
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

        private Transform _buildingPlacementFeedback;
        private Transform _buildingPlacementBeacon;

        private BuildableClickAndDrag _clickAndDrag;

        /// <summary>
        /// Only show/hide slot sprite renderer.  Always show boost feedback.
        /// </summary>
        public bool IsVisible
        {
            get { return _renderer.gameObject.activeSelf; }
            set { _renderer.gameObject.SetActive(value); }
        }

        public void controlBuildingPlacementFeedback(bool active)   // let's describe a publicly-accessible function that returns nothing, 
                                                                    // named contorlBuildingPlacementFeedback
                                                                    // which will be passed a bool (true or false) and active??
        {
            _renderer.gameObject.SetActive(active);
            _buildingPlacementFeedback.gameObject.SetActive(active);
            Invoke("stopBuildingPlacementFeedback", _buildingPlacementFeedback.GetComponent<ParticleSystem>().main.duration);
            _buildingPlacementBeacon.gameObject.SetActive(false);

        }

        public void stopBuildingPlacementFeedback() {
            _buildingPlacementFeedback.gameObject.SetActive(false);
            _renderer.gameObject.SetActive(false);
        }

        public void controlBuildingPlacementBeacon(bool active)
        {
            if (_renderer.gameObject.activeSelf && _buildingPlacementBeacon != null)
            {
                _buildingPlacementBeacon.gameObject.SetActive(active);
            }
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
            _explosion = _explosionController.Initialise();
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


            _buildingPlacementFeedback = _renderer.gameObject.transform.Find("BuildingPlacedFeedback");
            _buildingPlacementBeacon = _renderer.gameObject.transform.Find("BuildingPlacementBeacon");
            _clickAndDrag = GameObject.Find("BuildableClickAndDrag").GetComponentInChildren<BuildableClickAndDrag>();
        }


        public void OnDrop(PointerEventData eventData) {
            OnPointerClick(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
         //do nothing here
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Input.GetMouseButton(0) && _clickAndDrag.ClickAndDraging)//if we are doing click and drag
            {
                controlBuildingPlacementBeacon(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            controlBuildingPlacementBeacon(false);
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
            _explosion.Activate(Transform.Position);
            Invoke("NullifySlotBuilding", 1f);
            
		}

        private void NullifySlotBuilding()
        {
            SlotBuilding = null;
            //Debug.Log("Nullified slot");
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
