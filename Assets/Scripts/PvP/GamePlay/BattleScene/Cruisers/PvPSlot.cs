using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.BuildingPlacement;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.Feedback;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using BattleCruisers.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using System;
using System.Collections.ObjectModel;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using Unity.Netcode;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots
{
    public class PvPSlot : NetworkBehaviour, IPvPSlot, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler, IDragHandler
    {
        private IPvPCruiser _parentCruiser;
        private SpriteRenderer _renderer;
        private IPvPExplosion _explosion;
        public PvPExplosionController _explosionController;
        private IPvPBuildingPlacer _buildingPlacer;
        private Vector2 _size;
        // Hold reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private PvPSlotBoostFeedbackMonitor _boostFeedbackMonitor;
#pragma warning restore CS0414  // Variable is assigned but never used

        public PvPSlotType type;
        public PvPSlotType Type => type;

        public PvPBuildingFunction buildingFunctionAffinity;
        public PvPBuildingFunction BuildingFunctionAffinity => buildingFunctionAffinity;

        public PvPDirection direction;
        public PvPDirection Direction => direction;

        public float index;
        public float Index => index;

        public bool IsFree => _baseBuilding.Value == null;
        public ObservableCollection<IPvPBoostProvider> BoostProviders { get; private set; }
        public ReadOnlyCollection<IPvPSlot> NeighbouringSlots { get; private set; }
        public IPvPTransform Transform { get; private set; }
        public Vector3 BuildingPlacementPoint { get; private set; }
        public Vector2 Position => transform.position;

        private IPvPSettableBroadcastingProperty<IPvPBuilding> _baseBuilding;
        public IPvPBroadcastingProperty<IPvPBuilding> Building { get; private set; }

        private Transform _buildingPlacementFeedback;
        private Transform _buildingPlacementBeacon;

        private PvPBuildableClickAndDrag _clickAndDrag;

        /// <summary>
        /// Only show/hide slot sprite renderer.  Always show boost feedback.
        /// </summary>
        public bool IsVisible
        {
            get { return _renderer.gameObject.activeSelf; }
            set
            {
                _renderer.gameObject.SetActive(value);
                if (IsServer)
                    pvp_IsVisible.Value = value;
            }
        }



        private const bool pvp_b_InitialValue = false;
        public NetworkVariable<bool> pvp_IsVisible = new NetworkVariable<bool>();


        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                pvp_IsVisible.Value = pvp_b_InitialValue;
            }
            else
            {
                Initialise_Client();
                if (pvp_IsVisible.Value != pvp_b_InitialValue)
                {
                    Debug.Log($"NetworkVariable was {pvp_IsVisible.Value} upon being spawned" + $" when it should have been {pvp_b_InitialValue}");
                }
                else
                {
                    Debug.Log($"NetworkVariable is {pvp_IsVisible.Value} when spawned.");
                    IsVisible = pvp_IsVisible.Value;
                }
                pvp_IsVisible.OnValueChanged += OnPvPIsVisibleValueChanged;
            }
        }

        public override void OnNetworkDespawn()
        {
            pvp_IsVisible.OnValueChanged -= OnPvPIsVisibleValueChanged;
        }


        private void Initialise_Client()
        {
            _renderer = transform.FindNamedComponent<SpriteRenderer>("SlotImage");
        }


        private void OnPvPIsVisibleValueChanged(bool previous, bool current)
        {
            if (IsClient && IsOwner)
            {
                IsVisible = current;
            }
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

        public void stopBuildingPlacementFeedback()
        {
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

        private IPvPBuilding SlotBuilding
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

        public void Initialise(IPvPCruiser parentCruiser, ReadOnlyCollection<IPvPSlot> neighbouringSlots, IPvPBuildingPlacer buildingPlacer)
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

            BoostProviders = new ObservableCollection<IPvPBoostProvider>();

            Transform = new PvPTransformBC(transform);

            _baseBuilding = new PvPSettableBroadcastingProperty<IPvPBuilding>(initialValue: null);
            Building = new PvPBroadcastingProperty<IPvPBuilding>(_baseBuilding);

            PvPSlotBoostFeedbackMonitorInitialiser boostFeedbackInitialiser = GetComponentInChildren<PvPSlotBoostFeedbackMonitorInitialiser>();
            Assert.IsNotNull(boostFeedbackInitialiser);
            _boostFeedbackMonitor = boostFeedbackInitialiser.CreateFeedbackMonitor(this);


            _buildingPlacementFeedback = _renderer.gameObject.transform.Find("BuildingPlacedFeedback");
            _buildingPlacementBeacon = _renderer.gameObject.transform.Find("BuildingPlacementBeacon");
            _clickAndDrag = GameObject.Find("BuildableClickAndDrag").GetComponentInChildren<PvPBuildableClickAndDrag>();
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

        public PvPHighlightArgs CreateHighlightArgs(IPvPHighlightArgsFactory highlightArgsFactory)
        {
            return highlightArgsFactory.CreateForInGameObject(Transform.Position, _size);
        }

        public void SetBuilding(IPvPBuilding building)
        {
            Assert.IsNotNull(building);
            SlotBuilding = building;
        }


    }
}
