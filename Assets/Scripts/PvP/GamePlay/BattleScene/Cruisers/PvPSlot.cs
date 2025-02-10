using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.BuildingPlacement;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.Feedback;
using BattleCruisers.Utils;
using System;
using System.Collections.ObjectModel;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using Unity.Netcode;
using System.Linq;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.Properties;
using BattleCruisers.Tutorial.Highlighting;

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

        public bool IsFree
        {
            get
            {
                if (NetworkManager.Singleton.IsHost)
                {
                    pvp_IsFree.Value = _baseBuilding.Value == null;
                    return _baseBuilding.Value == null;
                }
                else
                {
                    return pvp_IsFree.Value;
                }
            }
        }
        public NetworkVariable<int> pvp_BoostProviders_Num = new NetworkVariable<int>();
        public ObservableCollection<IPvPBoostProvider> BoostProviders { get; private set; }
        public ReadOnlyCollection<IPvPSlot> NeighbouringSlots { get; private set; }
        public ITransform Transform { get; private set; }
        private Vector3 _buildingPlacementPoint;
        public Vector3 BuildingPlacementPoint
        {
            get
            {
                Transform buildingPlacementPoint = transform.FindNamedComponent<Transform>("BuildingPlacementPoint");
                _buildingPlacementPoint = buildingPlacementPoint.position;
                return _buildingPlacementPoint;
            }
            set
            {
                _buildingPlacementPoint = value;
            }
        }
        public Vector2 Position => transform.position;

        private ISettableBroadcastingProperty<IPvPBuilding> _baseBuilding;
        public IBroadcastingProperty<IPvPBuilding> Building { get; private set; }

        private Transform _buildingPlacementFeedback;
        private Transform _buildingPlacementBeacon;
        private PvPBuildableClickAndDrag _clickAndDrag;

        /// <summary>
        /// Only show/hide slot sprite renderer.  Always show boost feedback.
        /// </summary>
        public bool IsVisibleRederer
        {
            get { return _renderer.gameObject.activeSelf; }
            set
            {
                if (_renderer != null)
                    _renderer.gameObject.SetActive(value);
                /*                if (IsHost)
                                    pvp_IsVisibleRenderer.Value = value;*/
            }
        }

        public NetworkVariable<bool> pvp_IsFree = new NetworkVariable<bool>();
        public NetworkVariable<bool> pvp_IsVisibleRenderer = new NetworkVariable<bool>();
        private const bool pvp_b_InitialValue = false;
        private PvPBuildableOutlineController _outline;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                pvp_IsFree.Value = true;
                /*                pvp_IsVisibleRenderer.Value = pvp_b_InitialValue;*/
            }
            else
            {
                Initialise_Client();
                /*                if (pvp_IsVisibleRenderer.Value != pvp_b_InitialValue)
                                {
                                    Debug.Log($"NetworkVariable was {pvp_IsVisibleRenderer.Value} upon being spawned" + $" when it should have been {pvp_b_InitialValue}");
                                }
                                else
                                {
                                    Debug.Log($"NetworkVariable is {pvp_IsVisibleRenderer.Value} when spawned.");
                                    IsVisibleRederer = pvp_IsVisibleRenderer.Value;
                                }
                                pvp_IsVisibleRenderer.OnValueChanged += OnPvPIsVisibleRendererValueChanged;*/
            }
        }

        public override void OnNetworkDespawn()
        {
            //    pvp_IsVisibleRenderer.OnValueChanged -= OnPvPIsVisibleRendererValueChanged;
        }


        private void Initialise_Client()
        {
            _renderer = transform.FindNamedComponent<SpriteRenderer>("SlotImage");
            // _renderer.gameObject.SetActive(false);
        }


        private void OnPvPIsVisibleRendererValueChanged(bool previous, bool current)
        {
            if (IsClient && IsOwner)
                IsVisibleRederer = current;
        }

        public void controlBuildingPlacementFeedback(bool active)   // let's describe a publicly-accessible function that returns nothing, 
                                                                    // named contorlBuildingPlacementFeedback
                                                                    // which will be passed a bool (true or false) and active??
        {
            //    controlBuildingPlacementFeedbackClientRpc(active);
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
        public NetworkVariable<ulong> pvp_Building_NetworkObjectID = new NetworkVariable<ulong>();
        private IPvPBuilding SlotBuilding
        {
            set
            {
                if (_baseBuilding.Value != null)
                {
                    _baseBuilding.Value.Destroyed -= OnBuildingDestroyed;
                }

                _baseBuilding.Value = value;

                if (_baseBuilding.Value != null)
                {
                    // pvp
                    ulong objectId = ulong.MaxValue;
                    if (_baseBuilding.Value.GameObject.GetComponent<PvPBuilding>() != null)
                    {
                        objectId = (ulong)(_baseBuilding.Value.GameObject.GetComponent<PvPBuilding>()?._parent?.GetComponent<NetworkObject>()?.NetworkObjectId);
                    }

                    if (_baseBuilding.Value.GameObject.GetComponent<PvPUnit>() != null)
                    {
                        objectId = (ulong)(_baseBuilding.Value.GameObject.GetComponent<PvPUnit>()?._parent?.GetComponent<NetworkObject>()?.NetworkObjectId);
                    }

                    pvp_Building_NetworkObjectID.Value = objectId;
                    _buildingPlacer.PlaceBuilding(_baseBuilding.Value, this);
                    _baseBuilding.Value.Destroyed += OnBuildingDestroyed;
                    if (IsHost)
                        pvp_IsFree.Value = false;
                }
                else
                {
                    if (IsHost)
                    {
                        pvp_Building_NetworkObjectID.Value = 0;
                        pvp_IsFree.Value = true;
                    }
                }
            }
        }

        private void SetSlotBuildingOutline(PvPBuildableOutlineController outline)
        {
            _buildingPlacer.PlaceOutline(outline, this);
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
            SpriteRenderer slotRenderer = transform.FindNamedComponent<SpriteRenderer>("SlotImage");
            _size = slotRenderer.bounds.size;
            BoostProviders = new ObservableCollection<IPvPBoostProvider>();
            // pvp
            BoostProviders.CollectionChanged += (sender, e) =>
            {
                pvp_BoostProviders_Num.Value = BoostProviders.Count;
            };

            Transform = new TransformBC(transform);
            _baseBuilding = new PvPSettableBroadcastingProperty<IPvPBuilding>(initialValue: null);
            Building = new PvPBroadcastingProperty<IPvPBuilding>(_baseBuilding);

            PvPSlotBoostFeedbackMonitorInitialiser boostFeedbackInitialiser = GetComponentInChildren<PvPSlotBoostFeedbackMonitorInitialiser>();
            Assert.IsNotNull(boostFeedbackInitialiser);
            _boostFeedbackMonitor = boostFeedbackInitialiser.CreateFeedbackMonitor(this, IsHost);
            _buildingPlacementFeedback = _renderer.gameObject.transform.Find("BuildingPlacedFeedback");
            _buildingPlacementBeacon = _renderer.gameObject.transform.Find("BuildingPlacementBeacon");
            _clickAndDrag = GameObject.Find("BuildableClickAndDrag").GetComponentInChildren<PvPBuildableClickAndDrag>();
        }


        public void OnDrop(PointerEventData eventData)
        {
            if (IsClient && IsOwner)
                OnPointerClick(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsClient && IsOwner)
            {
                if (Input.GetMouseButton(0) && _clickAndDrag.ClickAndDraging)//if we are doing click and drag
                {
                    controlBuildingPlacementBeacon(true);
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsClient && IsOwner)
            {
                controlBuildingPlacementBeacon(false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Logging.LogMethod(Tags.SLOTS);

            if (IsClient && IsOwner)
            {
                if (IsVisibleRederer && pvp_IsFree.Value)
                {
                    //  _parentCruiser.ConstructSelectedBuilding(this);
                    _outline = _parentCruiser.FactoryProvider.PrefabFactory.CreateOutline(_parentCruiser.SelectedBuildableOutlinePrefab);
                    SetSlotBuildingOutline(_outline);
                    controlBuildingPlacementFeedback(true);
                    IAudioClipWrapper _placementSound = new AudioClipWrapper(_outline.placementSound);
                    _parentCruiser.FactoryProvider.Sound.UISoundPlayer.PlaySound(_placementSound);

                    //  ServerRpc call
                    OnPointerClickServerRpc(gameObject.name);
                }
                Clicked?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnBuildingDestroyed(object sender, EventArgs e)
        {
            _explosion.Activate(Transform.Position);
            SlotBuilding = null;
            //    Invoke("NullifySlotBuilding", 1f);
            BuildingDestroyedClientRpc();
        }

        private void OnBuildingDestroyed_PvPClient()
        {
            _explosion.Activate(Transform.Position);
            //    SlotBuilding = null;
            //    Invoke("NullifySlotBuilding", 1f);
        }

        private void NullifySlotBuilding()
        {
            SlotBuilding = null;
        }

        public HighlightArgs CreateHighlightArgs(IHighlightArgsFactory highlightArgsFactory)
        {
            return highlightArgsFactory.CreateForInGameObject(Transform.Position, _size);
        }

        public void SetBuilding(IPvPBuilding building)
        {
            Assert.IsNotNull(building);
            SlotBuilding = building;
            OnBuildablePlaceOnSlotClientRpc();
        }

        [ServerRpc(RequireOwnership = true)]
        public void OnPointerClickServerRpc(string slotName, ServerRpcParams serverRpcParams = default)
        {
            var clientId = serverRpcParams.Receive.SenderClientId;
            if (NetworkManager.ConnectedClientsIds.Contains(clientId) && IsFree)
            {
                _parentCruiser.ConstructSelectedBuilding(_parentCruiser.GetSlotWrapperController()._slotsByName[slotName]);
            }
        }

        [ClientRpc]
        private void BuildingDestroyedClientRpc()
        {
            if (!IsHost)
                OnBuildingDestroyed_PvPClient();
        }

        [ClientRpc]
        private void OnBuildablePlaceOnSlotClientRpc()
        {
            _outline?.BuildableCreated?.Invoke();
        }
    }
}
