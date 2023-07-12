using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Fog;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using Unity.Netcode;
using System.Linq;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.AudioSources;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public class PvPCruiser : PvPTarget, IPvPCruiser, IComparableItem
    {
        protected IPvPUIManager _uiManager;
        protected IPvPCruiser _enemyCruiser;
        private SpriteRenderer _renderer;
        protected Collider2D _collider;
        private IPvPCruiserHelper _helper;
        private PvPSlotWrapperController _slotWrapperController;
        private IPvPClickHandler _clickHandler;
        private IPvPDoubleClickHandler<IPvPBuilding> _buildingDoubleClickHandler;
        private IPvPDoubleClickHandler<IPvPCruiser> _cruiserDoubleClickHandler;
        private IPvPAudioClipWrapper _selectedSound;
        // Keep reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private IPvPManagedDisposable _fogOfWarManager, _unitReadySignal, _droneFeedbackSound;
#pragma warning restore CS0414  // Variable is assigned but never used

        public string stringKeyBase;
        public int numOfDrones;
        public float yAdjustmentInM;
        public Vector2 trashTalkScreenPosition;


        // ITarget
        public override PvPTargetType TargetType => PvPTargetType.Cruiser;
        public override Color Color { set { _renderer.color = value; } }
        public override Vector2 Size => _collider.bounds.size;
        public override Vector2 DroneAreaPosition => new Vector2(Position.x, Position.y - Size.y / 4);

        private Vector2 _droneAreaSize;
        public override Vector2 DroneAreaSize => _droneAreaSize;


        // IComparableItem
        public string Description { get; private set; }
        public string Name { get; private set; }
        public Sprite Sprite => _renderer.sprite;

        // ICruiser
        public PvPBuildableOutlineController SelectedBuildableOutlinePrefab { get; set; }
        private IPvPBuildableWrapper<IPvPBuilding> _selectedBuildingPrefab;
        public IPvPBuildableWrapper<IPvPBuilding> SelectedBuildingPrefab
        {
            get
            {
                return _selectedBuildingPrefab;
            }
            set
            {
                _selectedBuildingPrefab = value;
                SelectedBuildableOutlinePrefab = FactoryProvider.PrefabFactory.GetOutline(new PvPBuildableOutlineKey(value.Buildable.PrefabName + "Outline"));
            }
        }
        public IPvPDroneConsumerProvider DroneConsumerProvider { get; private set; }
        public PvPDirection Direction { get; private set; }
        public float YAdjustmentInM => yAdjustmentInM;
        public Vector2 TrashTalkScreenPosition => trashTalkScreenPosition;
        public IPvPFactoryProvider FactoryProvider { get; private set; }
        public IPvPCruiserSpecificFactories CruiserSpecificFactories { get; private set; }
        private PvPFogOfWar _fog;
        public IPvPGameObject Fog => _fog;
        public IPvPRepairManager RepairManager { get; private set; }
        public int NumOfDrones => numOfDrones;
        public IPvPBuildProgressCalculator BuildProgressCalculator { get; private set; }
        public bool IsPlayerCruiser => Position.x < 0;
        public PvPCruiserDeathExplosion deathPrefab;
        public PvPCruiserDeathExplosion DeathPrefab => deathPrefab;


        // ICruiserController

        public bool IsAlive => !IsDestroyed;
        public IPvPSlotAccessor SlotAccessor { get; private set; }
        public IPvPSlotHighlighter SlotHighlighter { get; private set; }
        public IPvPSlotNumProvider SlotNumProvider { get; private set; }
        public IPvPDroneManager DroneManager { get; private set; }
        public IPvPDroneFocuser DroneFocuser { get; private set; }
        public IPvPCruiserBuildingMonitor BuildingMonitor { get; private set; }
        public IPvPCruiserUnitMonitor UnitMonitor { get; private set; }
        public IPvPPopulationLimitMonitor PopulationLimitMonitor { get; private set; }
        public IPvPUnitTargets UnitTargets { get; private set; }
        public IPvPTargetTracker BlockedShipsTracker { get; private set; }

        public event EventHandler<PvPBuildingStartedEventArgs> BuildingStarted;
        public event EventHandler<PvPBuildingCompletedEventArgs> BuildingCompleted;
        public event EventHandler<PvPBuildingDestroyedEventArgs> BuildingDestroyed;
        public event EventHandler Clicked;
        private int updateCnt = 0;
        public bool isPvPCruiser = true;

        // network variables

        public NetworkVariable<int> pvp_NumOfDrones = new NetworkVariable<int>();
        public NetworkVariable<bool> pvp_DroneNumIncreased = new NetworkVariable<bool>();
        public NetworkVariable<bool> pvp_IdleDronesStarted = new NetworkVariable<bool>();
        public NetworkVariable<bool> pvp_IdleDronesEnded = new NetworkVariable<bool>();
        public NetworkVariable<bool> pvp_popLimitReachedFeedback = new NetworkVariable<bool>();

        private IPvPBroadcastingProperty<bool> _CruiserHasActiveDrones;


        private void Start()
        {
            if (IsClient && IsOwner)
            {
                PvPBattleSceneGodClient.Instance.RegisterAsPlayer(this);
            }

            else if (IsClient && !IsOwner)
            {
                PvPBattleSceneGodClient.Instance.RegisterAsEnemy(this);
            }

            if (NetworkManager.Singleton.IsServer)
                _healthTracker.SetMaxHealth();
        }


        public async void Initialise_Client_PvP(IPvPFactoryProvider factoryProvider, IPvPUIManager uiManager, IPvPCruiserHelper helper)
        {
            //  if (IsClient && IsOwner)
            //    {
            FactoryProvider = factoryProvider;
            _uiManager = uiManager;
            _helper = helper;

            SlotAccessor = _slotWrapperController.Initialise(this);

            _clickHandler.SingleClick += _clickHandler_SingleClick;
            _clickHandler.DoubleClick += _clickHandler_DoubleClick;

            IPvPSoundKey selectedSoundKey = IsPlayerCruiser ? PvPSoundKeys.UI.Selected.FriendlyCruiser : PvPSoundKeys.UI.Selected.EnemyCruiser;
            _selectedSound = await FactoryProvider.Sound.SoundFetcher.GetSoundAsync(selectedSoundKey);
            if (IsClient && IsOwner)
            {
                BuildingMonitor = new PvPCruiserBuildingMonitor(this);
                UnitMonitor = new PvPCruiserUnitMonitor(BuildingMonitor);

                PvPUnitReadySignalInitialiser unitReadySignalInitialiser = GetComponentInChildren<PvPUnitReadySignalInitialiser>();
                Assert.IsNotNull(unitReadySignalInitialiser);
                _unitReadySignal = unitReadySignalInitialiser.CreateSignal(this);
                // PopulationLimitMonitor = new PvPPopulationLimitMonitor(UnitMonitor);
            }
            //   }
        }


        public override void StaticInitialise(ILocTable commonStrings)
        {
            base.StaticInitialise(commonStrings);

            Assert.IsNotNull(deathPrefab);

            _renderer = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(_renderer);

            _collider = GetComponent<Collider2D>();
            Assert.IsNotNull(_collider);

            _slotWrapperController = GetComponentInChildren<PvPSlotWrapperController>(includeInactive: true);
            Assert.IsNotNull(_slotWrapperController);
            _slotWrapperController.StaticInitialise();
            SlotNumProvider = _slotWrapperController;

            _fog = GetComponentInChildren<PvPFogOfWar>(includeInactive: true);
            Assert.IsNotNull(_fog);

            PvPClickHandlerWrapper clickHandlerWrapper = GetComponent<PvPClickHandlerWrapper>();
            Assert.IsNotNull(clickHandlerWrapper);
            _clickHandler = clickHandlerWrapper.GetClickHandler();
            Name = _commonStrings.GetString($"Cruisers/{stringKeyBase}Name");
            Description = _commonStrings.GetString($"Cruisers/{stringKeyBase}Description");



            BuildingMonitor = new PvPCruiserBuildingMonitor(this);
            UnitMonitor = new PvPCruiserUnitMonitor(BuildingMonitor);
            PopulationLimitMonitor = new PvPPopulationLimitMonitor(UnitMonitor);
            UnitTargets = new PvPUnitTargets(UnitMonitor);

            _droneAreaSize = new Vector2(Size.x, Size.y * 0.8f);
        }


        protected override void CallRpc_ClickedRepairButton()
        {
            PvP_RepairableButtonClickedServerRpc();
        }


        public PvPSlotWrapperController GetSlotWrapperController()
        {
            return _slotWrapperController;
        }

        private void _droneManager_DroneNumChanged(object sender, PvPDroneNumChangedEventArgs e)
        {
            pvp_NumOfDrones.Value = DroneManager.NumOfDrones;
        }

        public async virtual void Initialise(IPvPCruiserArgs args)
        {
            Faction = args.Faction;
            // client rpc call
            PvP_SetFactionClientRpc(Faction);

            _enemyCruiser = args.EnemyCruiser;
            _uiManager = args.UiManager;
            DroneManager = args.DroneManager;
            DroneFocuser = args.DroneFocuser;
            // pvp code
            DroneManager.DroneNumChanged += _droneManager_DroneNumChanged;
            //......
            DroneManager.NumOfDrones = numOfDrones;
            DroneConsumerProvider = args.DroneConsumerProvider;
            FactoryProvider = args.FactoryProvider;
            CruiserSpecificFactories = args.CruiserSpecificFactories;
            Direction = args.FacingDirection;
            _helper = args.Helper;
            BuildProgressCalculator = args.BuildProgressCalculator;
            _buildingDoubleClickHandler = args.BuildingDoubleClickHandler;
            _cruiserDoubleClickHandler = args.CruiserDoubleClickHandler;
            _fogOfWarManager = args.FogOfWarManager;
            RepairManager = args.RepairManager;

            _fog.Initialise(args.FogStrength);

            SlotAccessor = _slotWrapperController.Initialise(this);
            SlotHighlighter = new PvPSlotHighlighter(SlotAccessor, args.HighlightableFilter, BuildingMonitor);

            PvPEnemyShipBlockerInitialiser enemyShipBlockerInitialiser = GetComponentInChildren<PvPEnemyShipBlockerInitialiser>();
            Assert.IsNotNull(enemyShipBlockerInitialiser);
            BlockedShipsTracker
                = enemyShipBlockerInitialiser.Initialise(
                    args.FactoryProvider.Targets,
                    args.CruiserSpecificFactories.Targets.TrackerFactory,
                    PvPHelper.GetOppositeFaction(Faction));

            /*            PvPUnitReadySignalInitialiser unitReadySignalInitialiser = GetComponentInChildren<PvPUnitReadySignalInitialiser>();
                        Assert.IsNotNull(unitReadySignalInitialiser);
                        _unitReadySignal = unitReadySignalInitialiser.CreateSignal(this);*/

            _CruiserHasActiveDrones = args.HasActiveDrones;
            _CruiserHasActiveDrones.ValueChanged += CruiserHasActiveDrones_ValueChanged;

            /*            PvPDroneSoundFeedbackInitialiser droneSoundFeedbackInitialiser = GetComponentInChildren<PvPDroneSoundFeedbackInitialiser>();
                        Assert.IsNotNull(droneSoundFeedbackInitialiser);
                        _droneFeedbackSound = droneSoundFeedbackInitialiser.Initialise(args.HasActiveDrones, FactoryProvider.SettingsManager);*/








            if (IsPlayerCruiser)
            {
                string logName = gameObject.name.ToUpper().Replace("(CLONE)", "");
#if LOG_ANALYTICS
    Debug.Log("Analytics: " + logName);
#endif
                IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
                try
                {
                    AnalyticsService.Instance.CustomData("Battle_Cruiser", applicationModel.DataProvider.GameModel.Analytics(applicationModel.Mode.ToString(), logName, applicationModel.UserWonSkirmish));
                    AnalyticsService.Instance.Flush();
                }
                catch (ConsentCheckException e)
                {
                    Debug.Log(e.Message);
                }

            }

        }


        private void CruiserHasActiveDrones_ValueChanged(object sender, EventArgs e)
        {
            PvP_PlayDroneFeedbackSoundClientRpc();
        }

        private void _clickHandler_SingleClick(object sender, EventArgs e)
        {
            // Logging.LogMethod(Tags.CRUISER);

            _uiManager.ShowCruiserDetails(this);
            _helper.FocusCameraOnCruiser(IsOwner, SynchedServerData.Instance.GetTeam());

            FactoryProvider.Sound.UISoundPlayer.PlaySound(_selectedSound);
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        private void _clickHandler_DoubleClick(object sender, EventArgs e)
        {
            _cruiserDoubleClickHandler.OnDoubleClick(this);
        }

        public Task<IPvPBuilding> ConstructBuilding(IPvPBuildableWrapper<IPvPBuilding> buildingPrefab, IPvPSlot slot)
        {
            // Logging.Log(Tags.CRUISER, buildingPrefab.Buildable.Name);

            SelectedBuildingPrefab = buildingPrefab;
            return ConstructSelectedBuilding(slot);
        }

        public async Task<IPvPBuilding> ConstructSelectedBuilding(IPvPSlot slot)
        {
            Assert.IsNotNull(SelectedBuildingPrefab);
            Assert.AreEqual(SelectedBuildingPrefab.Buildable.SlotSpecification.SlotType, slot.Type);
            IPvPBuilding building = await FactoryProvider.PrefabFactory.CreateBuilding(SelectedBuildingPrefab, _uiManager, FactoryProvider, OwnerClientId);

            building.Activate(
                new PvPBuildingActivationArgs(
                    this,
                    _enemyCruiser,
                    CruiserSpecificFactories,
                    slot,
                    _buildingDoubleClickHandler));

            slot.SetBuilding(building);

            building.CompletedBuildable += Building_CompletedBuildable;
            building.Destroyed += Building_Destroyed;

            building.StartConstruction();

            OnBuildingConstructionStarted(building, SlotAccessor, SlotHighlighter);

            BuildingStarted?.Invoke(this, new PvPBuildingStartedEventArgs(building));
            ulong objectId = (ulong)(building.GameObject.GetComponent<PvPBuildable<PvPBuildableActivationArgs>>()?._parent.GetComponent<NetworkObject>()?.NetworkObjectId);
            BuildingStartedClientRpc(objectId);

            slot.controlBuildingPlacementFeedback(true);

            if (IsPlayerCruiser)
            {
                string logName = building.PrefabName.ToUpper().Replace("(CLONE)", "");
#if LOG_ANALYTICS
    Debug.Log("Analytics: " + logName);
#endif
                IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
                try
                {
                    AnalyticsService.Instance.CustomData("Battle_Buildable", applicationModel.DataProvider.GameModel.Analytics(applicationModel.Mode.ToString(), logName, applicationModel.UserWonSkirmish));
                    AnalyticsService.Instance.Flush();
                }
                catch (ConsentCheckException ex)
                {
                    Debug.Log(ex.Message);
                }
            }

            return building;
        }

        private void OnBuildingConstructionStarted(IPvPBuilding buildingStarted, IPvPSlotAccessor slotAccessor, IPvPSlotHighlighter slotHighlighter)
        {
            if (!slotAccessor.IsSlotAvailableForPlayer(buildingStarted.SlotSpecification))
            {
                // _uiManager?.HideCurrentlyShownMenu();
                PvP_HideCurrentlyShownMenuClientRpc();
            }
            else
            {
                // Unhighlight the one slot that has just been taken
                slotHighlighter.HighlightAvailableSlots(buildingStarted.SlotSpecification);
            }
        }

        private void Building_CompletedBuildable(object sender, EventArgs e)
        {
            IPvPBuilding completedBuilding = sender.Parse<IPvPBuilding>();
            completedBuilding.CompletedBuildable -= Building_CompletedBuildable;

            BuildingCompleted?.Invoke(this, new PvPBuildingCompletedEventArgs(completedBuilding));
        }

        private void Building_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            e.DestroyedTarget.Destroyed -= Building_Destroyed;

            IPvPBuilding destroyedBuilding = e.DestroyedTarget.Parse<IPvPBuilding>();
            destroyedBuilding.CompletedBuildable -= Building_CompletedBuildable;

            BuildingDestroyed?.Invoke(this, new PvPBuildingDestroyedEventArgs(destroyedBuilding));
        }

        public virtual void Update()
        {
            if (RepairManager != null)
            {
                RepairManager.Repair(_time.DeltaTime);
            }
            //causing unnesacary updates
            /*updateCnt += 1;
            updateCnt = updateCnt%20;
            if (IsPlayerCruiser && updateCnt==0)
            {
                SlotHighlighter.HighlightAvailableSlotsCurrent();
            }*/
        }

        public void MakeInvincible()
        {
            _healthTracker.State = PvPHealthTrackerState.Immutable;
        }

        public virtual void AdjustStatsByDifficulty(Difficulty AIDifficulty)
        {

        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
            _CruiserHasActiveDrones.ValueChanged -= CruiserHasActiveDrones_ValueChanged;
            if (Faction == PvPFaction.Reds)
            {
                //Debug.Log(maxHealth);

                // BattleSceneGod.AddDeadBuildable(PvPTargetType, (int)(maxHealth));


                //Debug.Log(maxHealth);
                //BattleSceneGod.ShowDeadBuildableStats();
            }
        }

        public bool IsPvPCruiser()
        {
            return isPvPCruiser;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // Initialise_Client_PvP();
        }



        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

        }

        protected override void OnDamagedEventCalled(ulong objectId)
        {
            if (IsClient)
                base.OnDamagedEventCalled(objectId);
            if (IsServer)
                OnDamagedEventCalledClientRpc(objectId);
        }

        protected override void OnDestroyedEvent()
        {
            if (IsClient)
                base.OnDestroyedEvent();
            if (IsServer)
                OnDestroyedEventClientRpc();
        }

        [ClientRpc]
        private void OnDestroyedEventClientRpc()
        {
            OnDestroyedEvent();
        }
        [ServerRpc(RequireOwnership = true)]
        public void PvP_HighlightAvailableSlotsServerRpc(PvPSlotType SlotType, PvPBuildingFunction BuildingFunction, bool PreferFromFront, ServerRpcParams serverRpcParams = default)
        {
            PvPSlotSpecification SlotSpecification = new PvPSlotSpecification(SlotType, BuildingFunction, PreferFromFront);
            var clientId = serverRpcParams.Receive.SenderClientId;
            if (NetworkManager.ConnectedClientsIds.Contains(clientId))
            {
                bool wasAnySlotHighlighted = SlotHighlighter.HighlightAvailableSlots(SlotSpecification);
                if (!wasAnySlotHighlighted)
                {
                    PvP_PrioritisedSoundClientRpc(PvPSoundType.Events, "no-building-slots-left", PvPSoundPriority.VeryHigh);
                    SlotHighlighter.HighlightSlots(SlotSpecification);
                }
            }
        }


        [ClientRpc]
        private void PvP_PrioritisedSoundClientRpc(PvPSoundType soundType, string name, PvPSoundPriority priority)
        {

            FactoryProvider.Sound.PrioritisedSoundPlayer.PlaySound(new PvPPrioritisedSoundKey(new PvPSoundKey(soundType, name), priority));
        }

        [ServerRpc(RequireOwnership = true)]
        public void PvP_UnhighlightSlotsServerRpc()
        {
            SlotHighlighter.UnhighlightSlots();
        }


        [ServerRpc(RequireOwnership = true)]
        public void PvP_SelectedBuildingPrefabServerRpc(PvPBuildingCategory category, string prefabName, ServerRpcParams serverRpcParams = default)
        {
            var clientId = serverRpcParams.Receive.SenderClientId;
            if (NetworkManager.ConnectedClientsIds.Contains(clientId))
            {
                PvPBuildingKey buildingKey = new PvPBuildingKey(category, prefabName);
                SelectedBuildingPrefab = FactoryProvider.PrefabFactory.GetBuildingWrapperPrefab(buildingKey).UnityObject;
            }
        }

        [ClientRpc]
        private void PvP_SetFactionClientRpc(PvPFaction faction)
        {
            /*            if (IsOwner)
                            Faction = PvPFaction.Blues;
                        else
                            Faction = PvPFaction.Reds;*/
            Faction = faction;
        }

        [ServerRpc(RequireOwnership = true)]
        private void PvP_RepairableButtonClickedServerRpc()
        {
            IPvPDroneConsumer repairDroneConsumer = RepairManager.GetDroneConsumer(this);
            PvPPrioritisedSoundKey sound = DroneFocuser.ToggleDroneConsumerFocus(repairDroneConsumer, isTriggeredByPlayer: true);
            PvP_PrioritisedSoundClientRpc(sound.Key.Type, sound.Key.Name, sound.Priority);
        }

        [ClientRpc]
        private void PvP_PlayDroneFeedbackSoundClientRpc()
        {
            PvPDroneSoundFeedbackInitialiser droneSoundFeedbackInitialiser = GetComponentInChildren<PvPDroneSoundFeedbackInitialiser>();
            Assert.IsNotNull(droneSoundFeedbackInitialiser);
            AudioSource audioSource = droneSoundFeedbackInitialiser.gameObject.GetComponentInChildren<AudioSource>();

            IPvPAudioSource _audioSource = new PvPEffectVolumeAudioSource(
                        new PvPAudioSourceBC(audioSource),
                        PvPBattleSceneGodClient.Instance.factoryProvider.SettingsManager, 2);
            _audioSource?.Play(isSpatial: true);
            //   _droneFeedbackSound = droneSoundFeedbackInitialiser.Initialise(args.HasActiveDrones, FactoryProvider.SettingsManager);
        }

        [ClientRpc]
        private void PvP_HideCurrentlyShownMenuClientRpc()
        {
            if (IsOwner)
                _uiManager?.HideCurrentlyShownMenu();
        }

        [ClientRpc]
        private void OnDamagedEventCalledClientRpc(ulong objectId)
        {
            OnDamagedEventCalled(objectId);
        }

        [ClientRpc]
        private void BuildingStartedClientRpc(ulong objectId)
        {
            if (IsClient && IsOwner)
            {
                NetworkObject[] objs = FindObjectsByType<NetworkObject>(FindObjectsSortMode.None);
                foreach (NetworkObject obj in objs)
                {
                    if (obj.NetworkObjectId == objectId)
                    {
                        IPvPBuilding building = obj.gameObject.GetComponent<PvPBuildableWrapper<IPvPBuilding>>().Buildable.Parse<IPvPBuilding>();
                        BuildingStarted?.Invoke(this, new PvPBuildingStartedEventArgs(building));
                    }
                }
            }
        }

    }

    public enum Team { LEFT, RIGHT }

}
