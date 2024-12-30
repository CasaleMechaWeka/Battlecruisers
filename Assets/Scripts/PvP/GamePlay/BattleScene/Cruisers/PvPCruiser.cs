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
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System;
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
using UnityEngine.UI;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.PlatformAbstractions.Audio;

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
        private IAudioClipWrapper _selectedSound;
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
        public override Color Color { set { if (_renderer != null) _renderer.color = value; } }
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

        private int _variantIndexOfSelectedBuilding = -1;
        public int VariantIndexOfSelectedBuilding
        {
            get
            {
                return _variantIndexOfSelectedBuilding;
            }
            set
            {
                _variantIndexOfSelectedBuilding = value;
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
        public bool isPvPCruiser = true;

        // network variables

        public NetworkVariable<int> pvp_NumOfDrones = new NetworkVariable<int>();
        public NetworkVariable<bool> pvp_DroneNumIncreased = new NetworkVariable<bool>();
        public NetworkVariable<bool> pvp_IdleDronesStarted = new NetworkVariable<bool>();
        public NetworkVariable<bool> pvp_IdleDronesEnded = new NetworkVariable<bool>();
        public NetworkVariable<bool> pvp_popLimitReachedFeedback = new NetworkVariable<bool>();
        public NetworkVariable<byte> pvp_IsVictory = new NetworkVariable<byte>();

        private IPvPBroadcastingProperty<bool> _CruiserHasActiveDrones;
        private bool IsAIBotMode = false;

        protected virtual void Start()
        {
            if (IsAIBotMode)
            {
                PvPBattleSceneGodClient.Instance.RegisterAsEnemy(this);
            }
            else
            {
                if (IsClient && IsOwner)
                {
                    PvPBattleSceneGodClient.Instance.RegisterAsPlayer(this);
                }
                else if (IsClient && !IsOwner)
                {
                    PvPBattleSceneGodClient.Instance.RegisterAsEnemy(this);
                }
            }
            if (NetworkManager.Singleton.IsServer)
                _healthTracker.SetMaxHealth();

            if (Faction == PvPFaction.Blues)
            {
                PvPBattleSceneGodTunnel.AddAllBuildablesOfLeftPlayer(this.TargetType, PvPBattleSceneGodTunnel.difficultyDestructionScoreMultiplier * (float)maxHealth);
                PvPBattleSceneGodTunnel._playerACruiserVal = PvPBattleSceneGodTunnel.cruiser_scores[stringKeyBase];
                PvPBattleSceneGodTunnel._playerACruiserName = Name;
            }
            else
            {
                PvPBattleSceneGodTunnel.AddAllBuildablesOfRightPlayer(this.TargetType, PvPBattleSceneGodTunnel.difficultyDestructionScoreMultiplier * (float)maxHealth);
                PvPBattleSceneGodTunnel._playerBCruiserVal = PvPBattleSceneGodTunnel.cruiser_scores[stringKeyBase];
                PvPBattleSceneGodTunnel._playerBCruiserName = Name;
            }


        }


        private async Task LoadBodykit(int index)
        {
            Bodykit bodykit = await FactoryProvider.PrefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(index));
            if (bodykit != null)
            {
                GetComponent<SpriteRenderer>().sprite = bodykit.BodykitImage;
                // should update Name and Description for Bodykit
                Name = _commonStrings.GetString(ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.Bodykits[index].NameStringKeyBase);
                Description = _commonStrings.GetString(ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.Bodykits[index].DescriptionKeyBase);
            }
        }

        public async void Initialise_Client_PvP(IPvPFactoryProvider factoryProvider, IPvPUIManager uiManager, IPvPCruiserHelper helper)
        {
            if (!IsHost)
                FactoryProvider = factoryProvider;
            _uiManager = uiManager;
            _helper = helper;
            SlotAccessor = _slotWrapperController.Initialise(this);
            SlotHighlighter = new PvPSlotHighlighter(SlotAccessor, new PvPFreeSlotFilter(), BuildingMonitor);
            _clickHandler.SingleClick += _clickHandler_SingleClick;
            _clickHandler.DoubleClick += _clickHandler_DoubleClick;

            ISoundKey selectedSoundKey = IsPlayerCruiser ? SoundKeys.UI.Selected.FriendlyCruiser : SoundKeys.UI.Selected.EnemyCruiser;
            _selectedSound = await FactoryProvider.Sound.SoundFetcher.GetSoundAsync(selectedSoundKey);
            if (IsClient && IsOwner)
            {
                BuildingMonitor = new PvPCruiserBuildingMonitor(this);
                UnitMonitor = new PvPCruiserUnitMonitor(BuildingMonitor);

                PvPUnitReadySignalInitialiser unitReadySignalInitialiser = GetComponentInChildren<PvPUnitReadySignalInitialiser>();
                Assert.IsNotNull(unitReadySignalInitialiser);
                _unitReadySignal = unitReadySignalInitialiser.CreateSignal(this);
            }
            // apply bodykit here
            if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
            {
                int id_bodykit = IsOwner ? SynchedServerData.Instance.playerABodykit.Value : SynchedServerData.Instance.playerBBodykit.Value;
                if (id_bodykit != -1)
                {
                    await LoadBodykit(id_bodykit);
                }
            }
            else
            {
                int id_bodykit = IsOwner ? SynchedServerData.Instance.playerBBodykit.Value : SynchedServerData.Instance.playerABodykit.Value;
                if (id_bodykit != -1)
                {
                    await LoadBodykit(id_bodykit);
                }
            }
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

            if (IsClient)
                _cruiserDoubleClickHandler = new PvPPlayerCruiserDoubleClickHandler();
        }

        public void SetAIBotMode()
        {
            IsAIBotMode = true;
            _fog.SetAIBotMode();
        }

        public bool IsAIBot()
        {
            return IsAIBotMode;
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

        public virtual void Initialise(IPvPCruiserArgs args)
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

            _CruiserHasActiveDrones = args.HasActiveDrones;
            _CruiserHasActiveDrones.ValueChanged += CruiserHasActiveDrones_ValueChanged;
            /*if (IsPlayerCruiser)
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
            */
        }


        private void CruiserHasActiveDrones_ValueChanged(object sender, EventArgs e)
        {
            PvP_PlayDroneFeedbackSoundClientRpc();
        }

        private void _clickHandler_SingleClick(object sender, EventArgs e)
        {
            _uiManager.ShowCruiserDetails(this);
            if (!IsAIBotMode)
                _helper.FocusCameraOnCruiser(IsOwner, SynchedServerData.Instance.GetTeam());
            FactoryProvider.Sound.UISoundPlayer.PlaySound(_selectedSound);
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        private void _clickHandler_DoubleClick(object sender, EventArgs e)
        {
            _cruiserDoubleClickHandler.OnDoubleClick(this);
        }

        public IPvPBuilding ConstructBuilding(IPvPBuildableWrapper<IPvPBuilding> buildingPrefab, IPvPSlot slot)
        {
            SelectedBuildingPrefab = buildingPrefab;
            return ConstructSelectedBuilding(slot);
        }

        public IPvPBuilding ConstructSelectedBuilding(IPvPSlot slot)
        {
            Assert.IsNotNull(SelectedBuildingPrefab);
            Assert.AreEqual(SelectedBuildingPrefab.Buildable.SlotSpecification.SlotType, slot.Type);
            IPvPBuilding building = FactoryProvider.PrefabFactory.CreateBuilding(SelectedBuildingPrefab, _uiManager, FactoryProvider, OwnerClientId);

            Assert.IsNotNull(building);

            building.Activate(
                new PvPBuildingActivationArgs(
                    this,
                    _enemyCruiser,
                    CruiserSpecificFactories,
                    slot,
                    _buildingDoubleClickHandler,
                    VariantIndexOfSelectedBuilding));

            slot.SetBuilding(building);

            building.CompletedBuildable += Building_CompletedBuildable;
            building.Destroyed += Building_Destroyed;

            building.StartConstruction();

            OnBuildingConstructionStarted(building, SlotAccessor, SlotHighlighter);

            BuildingStarted?.Invoke(this, new PvPBuildingStartedEventArgs(building));

            ulong objectId = (ulong)(building.GameObject.GetComponent<PvPBuilding>()?._parent.GetComponent<NetworkObject>()?.NetworkObjectId);
            BuildingStartedClientRpc(objectId);

            slot.controlBuildingPlacementFeedback(true);

            /* if (IsPlayerCruiser)
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
            */

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
                if (IsOwner)
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

        public virtual void FixedUpdate()
        {
            if (RepairManager != null)
            {
                RepairManager.Repair(_time.DeltaTime);
            }
            if (_enemyCruiser != null && _enemyCruiser.IsAlive)
            {
                if (IsServer)
                {
                    if (Faction == PvPFaction.Blues)
                    {
                        PvPBattleSceneGodServer.AddPlayedTime_Left(PvPTargetType.PlayedTime, Time.deltaTime);
                    }
                    if (Faction == PvPFaction.Reds)
                    {
                        PvPBattleSceneGodServer.AddPlayedTime_Right(PvPTargetType.PlayedTime, Time.deltaTime);
                    }
                }
            }
            if (Faction == PvPFaction.Blues)
            {
                PvPBattleSceneGodTunnel._playerALevelTimeInSeconds += Time.deltaTime;
            }
            if (Faction == PvPFaction.Reds)
            {
                PvPBattleSceneGodTunnel._playerBLevelTimeInSeconds += Time.deltaTime;
            }
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
                PvPBattleSceneGodServer.AddDeadBuildable_Left(TargetType, (int)(maxHealth));
            }
            else
            {
                PvPBattleSceneGodServer.AddDeadBuildable_Right(TargetType, (int)(maxHealth));
            }
        }

        public bool IsPvPCruiser()
        {
            return isPvPCruiser;
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsClient)
                PvPBattleSceneGodClient.Instance.AddNetworkObject(GetComponent<NetworkObject>());
        }

        public override void OnNetworkDespawn()
        {
            if (IsClient)
                PvPBattleSceneGodClient.Instance.RemoveNetworkObject(GetComponent<NetworkObject>());
            base.OnNetworkDespawn();

            if (Faction == PvPFaction.Blues)
            {
                Image fillableImage = GameObject.Find("HUDCanvas/HealthBarPanel/PlayerLeftCruiserHealth/Foreground")?.GetComponent<Image>();
                if (fillableImage != null)
                {
                    fillableImage.fillAmount = 0f;
                }
            }
            else
            {
                Image fillableImage = GameObject.Find("HUDCanvas/HealthBarPanel/PlayerRightCruiserHealth/Foreground")?.GetComponent<Image>();
                if (fillableImage != null)
                    fillableImage.fillAmount = 0f;
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }


        protected override void OnDamagedEventCalled(ulong objectId)
        {
            if (IsServer)
                OnDamagedEventCalledClientRpc(objectId);
            else
                base.OnDamagedEventCalled(objectId);
        }

        protected override void InternalDestroy()
        {

        }

        protected override void OnDestroyedEvent()
        {
            if (Faction == PvPFaction.Blues)
            {
                PvPCaptainExoHUDController.Instance.DoLeftAngry();
                PvPCaptainExoHUDController.Instance.DoRightHappy();
            }
            else
            {
                PvPCaptainExoHUDController.Instance.DoLeftHappy();
                PvPCaptainExoHUDController.Instance.DoRightAngry();
            }
            SlotHighlighter.UnhighlightSlots();
            SpriteRenderer[] visuals = GameObject.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
            foreach (SpriteRenderer visual in visuals)
            {
                visual.enabled = false;
            }

            Collider2D[] colliders = GameObject.GetComponentsInChildren<Collider2D>(includeInactive: true);
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = false;
            }

            if (IsServer)
                OnDestroyedEventClientRpc();
            else
            {
                base.OnDestroyedEvent();
            }
        }

        protected override void OnHealthGone()
        {
            PvPBattleSceneGodClient.Instance.components.backgroundClickableEmitter.enabled = false;
            OnHealthGoneClientRpc();
            base.OnHealthGone();
        }

        [ClientRpc]
        private void OnHealthGoneClientRpc()
        {
            if (!IsHost)
            {
                PvPBattleSceneGodClient.Instance.components.backgroundClickableEmitter.enabled = false;
            }
        }

        [ClientRpc]
        private void OnDestroyedEventClientRpc()
        {
            if (!IsHost)
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
                    PvP_PrioritisedSoundClientRpc(SoundType.Events, "no-building-slots-left", SoundPriority.VeryHigh);
                    SlotHighlighter.HighlightSlots(SlotSpecification);
                }
            }
        }


        [ClientRpc]
        private void PvP_PrioritisedSoundClientRpc(SoundType soundType, string name, SoundPriority priority)
        {

            FactoryProvider.Sound.PrioritisedSoundPlayer.PlaySound(new PrioritisedSoundKey(new SoundKey(soundType, name), priority));
        }

        [ServerRpc(RequireOwnership = true)]
        public void PvP_UnhighlightSlotsServerRpc()
        {
            if (!IsDestroyed)
                SlotHighlighter?.UnhighlightSlots();
        }


        [ServerRpc(RequireOwnership = true)]
        public void PvP_SelectedBuildingPrefabServerRpc(PvPBuildingCategory category, string prefabName, int variantIndex, ServerRpcParams serverRpcParams = default)
        {
            var clientId = serverRpcParams.Receive.SenderClientId;
            if (NetworkManager.ConnectedClientsIds.Contains(clientId))
            {
                PvPBuildingKey buildingKey = new PvPBuildingKey(category, prefabName);
                SelectedBuildingPrefab = FactoryProvider.PrefabFactory.GetBuildingWrapperPrefab(buildingKey).UnityObject;
                VariantIndexOfSelectedBuilding = variantIndex;
            }
        }

        [ClientRpc]
        private void PvP_SetFactionClientRpc(PvPFaction faction)
        {
            Faction = faction;
        }

        [ServerRpc(RequireOwnership = true)]
        private void PvP_RepairableButtonClickedServerRpc()
        {
            IPvPDroneConsumer repairDroneConsumer = RepairManager.GetDroneConsumer(this);
            PrioritisedSoundKey sound = DroneFocuser.ToggleDroneConsumerFocus(repairDroneConsumer, isTriggeredByPlayer: true);
            PvP_PrioritisedSoundClientRpc(sound.Key.Type, sound.Key.Name, sound.Priority);
        }

        [ClientRpc]
        private void PvP_PlayDroneFeedbackSoundClientRpc()
        {
            PvPDroneSoundFeedbackInitialiser droneSoundFeedbackInitialiser = GetComponentInChildren<PvPDroneSoundFeedbackInitialiser>();
            if (droneSoundFeedbackInitialiser == null)
                return;
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
            if (!IsHost)
                OnDamagedEventCalled(objectId);
        }

        [ClientRpc]
        private void BuildingStartedClientRpc(ulong objectId)
        {
            if (!IsHost && IsOwner)
            {
                NetworkObject obj = PvPBattleSceneGodClient.Instance.GetNetworkObject(objectId);
                IPvPBuilding building = obj.gameObject.GetComponent<PvPBuildableWrapper<IPvPBuilding>>().Buildable.Parse<IPvPBuilding>();
                BuildingStarted?.Invoke(this, new PvPBuildingStartedEventArgs(building));
            }
        }
    }

    public enum Team { LEFT, RIGHT }

}
