using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Slots.Feedback;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using System.Collections.ObjectModel;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.UI;

namespace BattleCruisers.Cruisers.Slots
{
    public enum SlotType
    {
        // Explicitly set integer values, because the Unity inspector binds
        // to the integer values.  So now, if I decide to get rid of a slot
        // type (yet again), I don't need to adjust every single prefab 
        // that has a slot type field.  Thanks Manya!
        Utility = 1,
        Mast = 2,
        Bow = 3,
        Platform = 4,
        Deck = 5
    }

    public class Slot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler, IDragHandler, IClickableEmitter, IHighlightable
    {
        private ICruiser _parentCruiser;
        private SpriteRenderer _renderer;
        private IPoolable<Vector3> _explosion;
        public ExplosionController _explosionController;
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
        public ReadOnlyCollection<Slot> NeighbouringSlots { get; private set; }
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
            if (_buildingPlacementFeedback != null)
            {
                _buildingPlacementFeedback.gameObject.SetActive(active);
                ParticleSystem particleSystem = _buildingPlacementFeedback.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    Invoke("stopBuildingPlacementFeedback", particleSystem.main.duration);
                }
            }
            if (_buildingPlacementBeacon != null)
            {
                _buildingPlacementBeacon.gameObject.SetActive(false);
            }
        }

        public void stopBuildingPlacementFeedback()
        {
            if (_buildingPlacementFeedback != null)
            {
                _buildingPlacementFeedback.gameObject.SetActive(false);
            }
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
                    IBuilding building = _baseBuilding.Value;

                    // Calculate the building's world position based on slot placement point and puzzle root offset
                    float verticalChange = building.Position.y - building.PuzzleRootPoint.y;
                    float horizontalChange = building.Position.x - building.PuzzleRootPoint.x;

                    Vector3 targetWorldPosition = BuildingPlacementPoint
                                                + (Transform.Up * verticalChange)
                                                + (Transform.Right * horizontalChange);

                    // Parent the building to this slot so it follows slot movement and rotation
                    // Using worldPositionStays=true keeps the building at the same world position during reparenting
                    building.SetParent(transform, worldPositionStays: true);

                    // For child objects, set local rotation to identity so the building inherits the parent's rotation
                    // This ensures the building's world rotation matches the slot's world rotation
                    building.Transform.PlatformObject.localRotation = Quaternion.identity;

                    // Set the world position to the calculated target
                    building.Position = targetWorldPosition;

                    if (building.HealthBar.Offset.x == 0
                        || !Transform.IsMirroredAcrossYAxis)
                    {
                        building.HealthBar.Offset = building.HealthBar.Offset;
                    }
                    else
                    {
                        building.HealthBar.Offset = new Vector2(
                            -building.HealthBar.Offset.x,
                            building.HealthBar.Offset.y);
                    }

                    _baseBuilding.Value.Destroyed += OnBuildingDestroyed;
                }
            }
        }

        public event EventHandler Clicked;

        public void Initialise(ICruiser parentCruiser, ReadOnlyCollection<Slot> neighbouringSlots)
        {
            Helper.AssertIsNotNull(parentCruiser, neighbouringSlots);

            _parentCruiser = parentCruiser;
            NeighbouringSlots = neighbouringSlots;

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

            Transform singleBoostEffect = transform.FindNamedComponent<Transform>("BoostFeedback/SingleBoostEffect");
            Transform doubleBoostEffect = transform.FindNamedComponent<Transform>("BoostFeedback/DoubleBoostEffect");

            _boostFeedbackMonitor = new SlotBoostFeedbackMonitor(
                    this,
                    new BoostStateFinder(),
                    new BoostFeedback(
                        new GameObjectBC(singleBoostEffect.gameObject),
                        new GameObjectBC(doubleBoostEffect.gameObject)));

            _buildingPlacementFeedback = _renderer.gameObject.transform.Find("BuildingPlacedFeedback");
            _buildingPlacementBeacon = _renderer.gameObject.transform.Find("BuildingPlacementBeacon");
            _clickAndDrag = GameObject.Find("BuildableClickAndDrag").GetComponentInChildren<BuildableClickAndDrag>();
        }


        public void OnDrop(PointerEventData eventData)
        {
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

        public HighlightArgs CreateHighlightArgs(HighlightArgsFactory highlightArgsFactory)
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
