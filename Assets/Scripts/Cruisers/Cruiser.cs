using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Pools;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Cruisers.Fog;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    public class Cruiser : Target, ICruiser, IComparableItem
    {
        protected readonly float UltraCruiserUtilityModifier = 2.0f; //UltraCruiser base utility stat modifier
        protected readonly float UltraCruiserHealthModifier = 1.25f; //UltraCruiser base health stat modifier        
        protected UIManager _uiManager;
        protected ICruiser _enemyCruiser;
        private SpriteRenderer _renderer;
        protected Collider2D _collider;
        private ICruiserHelper _helper;
        private SlotWrapperController _slotWrapperController;
        private ClickHandler _clickHandler;
        private IDoubleClickHandler<IBuilding> _buildingDoubleClickHandler;
        private IDoubleClickHandler<ICruiser> _cruiserDoubleClickHandler;
        private AudioClipWrapper _selectedSound;
        // Keep reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private IManagedDisposable _fogOfWarManager, _unitReadySignal, _droneFeedbackSound;
#pragma warning restore CS0414  // Variable is assigned but never used
        private SettingsManager settingsManager;

        public string stringKeyBase;
        public int numOfDrones;
        public float yAdjustmentInM;
        public Vector2 trashTalkScreenPosition;
        public HullType hullType;

        // ITarget
        public override TargetType TargetType => TargetType.Cruiser;
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
        public IBuildableWrapper<IBuilding> SelectedBuildingPrefab { get; set; }
        public IDroneConsumerProvider DroneConsumerProvider { get; private set; }
        public Direction Direction { get; private set; }
        public float YAdjustmentInM => yAdjustmentInM;
        public Vector2 TrashTalkScreenPosition => trashTalkScreenPosition;
        public CruiserSpecificFactories CruiserSpecificFactories { get; private set; }
        private FogOfWar _fog;
        public IGameObject Fog => _fog;
        public IRepairManager RepairManager { get; private set; }
        public int NumOfDrones => numOfDrones;
        public IBuildProgressCalculator BuildProgressCalculator { get; private set; }
        public bool IsPlayerCruiser => Position.x < 0;
        public CruiserDeathExplosion deathPrefab;
        public CruiserDeathExplosion DeathPrefab => deathPrefab;


        // ICruiserController
        public bool IsAlive => !IsDestroyed;
        public SlotAccessor SlotAccessor { get; private set; }
        public SlotHighlighter SlotHighlighter { get; private set; }
        public ISlotNumProvider SlotNumProvider { get; private set; }
        public DroneManager DroneManager { get; private set; }
        public IDroneFocuser DroneFocuser { get; private set; }
        public ICruiserBuildingMonitor BuildingMonitor { get; private set; }
        public ICruiserUnitMonitor UnitMonitor { get; private set; }
        public IPopulationLimitMonitor PopulationLimitMonitor { get; private set; }
        public IUnitTargets UnitTargets { get; private set; }
        public TargetTracker BlockedShipsTracker { get; private set; }

        public event EventHandler<BuildingStartedEventArgs> BuildingStarted;
        public event EventHandler<BuildingCompletedEventArgs> BuildingCompleted;
        public event EventHandler<BuildingDestroyedEventArgs> BuildingDestroyed;
        public event EventHandler Clicked;
        public bool isCruiser = true;
        public bool isUsingBodykit = false;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            Assert.IsNotNull(deathPrefab);

            _renderer = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(_renderer);

            _collider = GetComponent<Collider2D>();
            Assert.IsNotNull(_collider);


            _slotWrapperController = GetComponentInChildren<SlotWrapperController>(includeInactive: true);
            Assert.IsNotNull(_slotWrapperController);
            _slotWrapperController.StaticInitialise();
            SlotNumProvider = _slotWrapperController;

            _fog = GetComponentInChildren<FogOfWar>(includeInactive: true);
            Assert.IsNotNull(_fog);

            ClickHandlerWrapper clickHandlerWrapper = GetComponent<ClickHandlerWrapper>();
            Assert.IsNotNull(clickHandlerWrapper);
            _clickHandler = clickHandlerWrapper.GetClickHandler();
            Name = LocTableCache.CommonTable.GetString($"Cruisers/{stringKeyBase}Name");
            Description = LocTableCache.CommonTable.GetString($"Cruisers/{stringKeyBase}Description");

            BuildingMonitor = new CruiserBuildingMonitor(this);
            UnitMonitor = new CruiserUnitMonitor(BuildingMonitor);
            PopulationLimitMonitor = new PopulationLimitMonitor(UnitMonitor);
            UnitTargets = new UnitTargets(UnitMonitor);

            _droneAreaSize = new Vector2(Size.x, Size.y * 0.8f);
        }

        public async virtual void Initialise(CruiserArgs args)
        {
            Faction = args.Faction;
            _enemyCruiser = args.EnemyCruiser;
            _uiManager = args.UiManager;
            DroneManager = args.DroneManager;
            DroneFocuser = args.DroneFocuser;
            DroneManager.NumOfDrones = numOfDrones;
            DroneConsumerProvider = args.DroneConsumerProvider;
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
            SlotHighlighter = new SlotHighlighter(SlotAccessor, args.HighlightableFilter, BuildingMonitor);

            EnemyShipBlockerInitialiser enemyShipBlockerInitialiser = GetComponentInChildren<EnemyShipBlockerInitialiser>();
            Assert.IsNotNull(enemyShipBlockerInitialiser);
            BlockedShipsTracker
                = enemyShipBlockerInitialiser.Initialise(
                    FactoryProvider.Targets,
                    args.CruiserSpecificFactories.Targets.TrackerFactory,
                    Helper.GetOppositeFaction(Faction));

            UnitReadySignalInitialiser unitReadySignalInitialiser = GetComponentInChildren<UnitReadySignalInitialiser>();
            Assert.IsNotNull(unitReadySignalInitialiser);
            _unitReadySignal = unitReadySignalInitialiser.CreateSignal(this);

            DroneSoundFeedbackInitialiser droneSoundFeedbackInitialiser = GetComponentInChildren<DroneSoundFeedbackInitialiser>();
            Assert.IsNotNull(droneSoundFeedbackInitialiser);
            _droneFeedbackSound = droneSoundFeedbackInitialiser.Initialise(args.HasActiveDrones);

            SoundKey selectedSoundKey = IsPlayerCruiser ? SoundKeys.UI.Selected.FriendlyCruiser : SoundKeys.UI.Selected.EnemyCruiser;
            _selectedSound = await SoundFetcher.GetSoundAsync(selectedSoundKey);

            _clickHandler.SingleClick += _clickHandler_SingleClick;
            _clickHandler.DoubleClick += _clickHandler_DoubleClick;

            Debug.Log("HealthGainPerDroneS: " + HealthGainPerDroneS);


            // RICH MODE FOR PREMIUM (ONLY FOR PVE!!!)
            settingsManager = DataProvider.SettingsManager;
            if (settingsManager.RichMode)
            {
                DroneManager.NumOfDrones = numOfDrones * 4;
            }

            if (IsPlayerCruiser)
            {
                string logName = gameObject.name.ToUpper().Replace("(CLONE)", "");
                int id_bodykit = DataProvider.GameModel.PlayerLoadout.SelectedBodykit;

                if (id_bodykit != -1)
                {
                    Bodykit bodykit = PrefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(id_bodykit));
                    if (bodykit.cruiserType == hullType)
                    {
                        GetComponent<SpriteRenderer>().sprite = bodykit.BodykitImage;
                        // should update Name and Description for Bodykit
                        Name = LocTableCache.CommonTable.GetString(StaticData.Bodykits[id_bodykit].NameStringKeyBase);
                        Description = LocTableCache.CommonTable.GetString(StaticData.Bodykits[id_bodykit].DescriptionKeyBase);
                        isUsingBodykit = true;
                    }
                }
                /*#if LOG_ANALYTICS
                    Debug.Log("Analytics: " + logName);
                #endif
                                ApplicationModel applicationModel = ApplicationModel;
                                try
                                {
                                    AnalyticsService.Instance.CustomData("Battle_Cruiser", DataProvider.GameModel.Analytics(ApplicationModel.Mode.ToString(), logName, ApplicationModel.UserWonSkirmish));
                                    AnalyticsService.Instance.Flush();
                                }
                                catch(ConsentCheckException e)
                                {
                                    Debug.Log(e.Message);
                                }*/

            }
            else
            {
                // AI bot
                if (ApplicationModel.Mode == GameMode.CoinBattle)
                {
                    int id_bodykit = DataProvider.GameModel.ID_Bodykit_AIbot;
                    Debug.Log(id_bodykit);
                    if (id_bodykit != -1)
                    {
                        Bodykit bodykit = PrefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(id_bodykit));
                        if (bodykit.cruiserType == hullType)
                        {
                            GetComponent<SpriteRenderer>().sprite = bodykit.BodykitImage;
                            Name = LocTableCache.CommonTable.GetString(StaticData.Bodykits[id_bodykit].NameStringKeyBase);
                            Description = LocTableCache.CommonTable.GetString(StaticData.Bodykits[id_bodykit].DescriptionKeyBase);
                            isUsingBodykit = true;
                        }
                    }
                }
            }
        }
        private void _clickHandler_SingleClick(object sender, EventArgs e)
        {
            Logging.LogMethod(Tags.CRUISER);

            _uiManager.ShowCruiserDetails(this);
            _helper.FocusCameraOnCruiser();
            FactoryProvider.Sound.UISoundPlayer.PlaySound(_selectedSound);

            Clicked?.Invoke(this, EventArgs.Empty);
        }

        private void _clickHandler_DoubleClick(object sender, EventArgs e)
        {
            _cruiserDoubleClickHandler.OnDoubleClick(this);
        }

        public IBuilding ConstructBuilding(IBuildableWrapper<IBuilding> buildingPrefab, Slot slot)
        {
            Logging.Log(Tags.CRUISER, buildingPrefab.Buildable.Name);

            SelectedBuildingPrefab = buildingPrefab;
            return ConstructSelectedBuilding(slot);
        }

        public IBuilding ConstructSelectedBuilding(Slot slot)
        {
            Assert.IsNotNull(SelectedBuildingPrefab);
            Assert.AreEqual(SelectedBuildingPrefab.Buildable.SlotSpecification.SlotType, slot.Type);
            IBuilding building = PrefabFactory.CreateBuilding(SelectedBuildingPrefab);
            /*       SetVariantIcon(building);*/
            building.Activate(
                new BuildingActivationArgs(
                    this,
                    _enemyCruiser,
                    CruiserSpecificFactories,
                    slot,
                    _buildingDoubleClickHandler));

            slot.SetBuilding(building);

            building.CompletedBuildable += Building_CompletedBuildable;
            building.Destroyed += Building_Destroyed;

            building.StartConstruction();
            _helper.OnBuildingConstructionStarted(building, SlotAccessor, SlotHighlighter);

            BuildingStarted?.Invoke(this, new BuildingStartedEventArgs(building));

            slot.controlBuildingPlacementFeedback(true);

            if (IsPlayerCruiser)
            {
                string logName = building.PrefabName.ToUpper().Replace("(CLONE)", "");
                /*#if LOG_ANALYTICS
                    Debug.Log("Analytics: " + logName);
                #endif
                                ApplicationModel applicationModel = ApplicationModel;
                                try
                                {
                                    AnalyticsService.Instance.CustomData("Battle_Buildable", DataProvider.GameModel.Analytics(ApplicationModel.Mode.ToString(), logName, ApplicationModel.UserWonSkirmish));                    
                                    AnalyticsService.Instance.Flush();
                                }
                                catch (ConsentCheckException ex)
                                {
                                    Debug.Log(ex.Message);
                                }*/
            }

            return building;
        }



        private void Building_CompletedBuildable(object sender, EventArgs e)
        {
            IBuilding completedBuilding = sender.Parse<IBuilding>();
            completedBuilding.CompletedBuildable -= Building_CompletedBuildable;

            BuildingCompleted?.Invoke(this, new BuildingCompletedEventArgs(completedBuilding));
        }

        private void Building_Destroyed(object sender, DestroyedEventArgs e)
        {
            e.DestroyedTarget.Destroyed -= Building_Destroyed;

            IBuilding destroyedBuilding = e.DestroyedTarget.Parse<IBuilding>();
            destroyedBuilding.CompletedBuildable -= Building_CompletedBuildable;

            BuildingDestroyed?.Invoke(this, new BuildingDestroyedEventArgs(destroyedBuilding));
        }

        public virtual void FixedUpdate()
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

            if (IsPlayerCruiser && _enemyCruiser.IsAlive)
                BattleSceneGod.AddPlayedTime(TargetType.PlayedTime, _time.DeltaTime);
        }

        public void MakeInvincible()
        {
            _healthTracker.State = HealthTrackerState.Immutable;
        }

        public void MakeDamagable()
        {
            _healthTracker.State = HealthTrackerState.Mutable;
        }

        public virtual void AdjustStatsByDifficulty(Difficulty AIDifficulty)
        {

        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
            if (Faction == Faction.Reds)
            {
                //Debug.Log(maxHealth);
                BattleSceneGod.AddDeadBuildable(TargetType, (int)(maxHealth));
                //Debug.Log(maxHealth);
                //BattleSceneGod.ShowDeadBuildableStats();
            }
        }

        public bool IsCruiser()
        {
            return isCruiser;
        }
        /// <summary>
        /// 
        /// </summary>
        protected void SetUltraCruiserHealth(CruiserArgs args)
        {
            if (args.Faction == Faction.Reds)
            {
                float health = UltraCruiserHealthModifier * this.MaxHealth;
                this._healthTracker.MaxHealth = health;
                //Debug.Log("Enemy health value: "+health);
            }
            else
            {
                this._healthTracker.MaxHealth = base.maxHealth;
                //Debug.Log("Player health value: "+maxHealth);
            }
        }

        protected float SetUltraCruiserUtility(CruiserArgs args, float cruiserBaseUtility)
        {
            if (args.Faction == Faction.Reds)
            {
                //Debug.Log("Enemy utility value: "+cruiserBaseUtility * UltraCruiserUtilityModifier);
                return cruiserBaseUtility * UltraCruiserUtilityModifier;
            }
            else
            {
                //Debug.Log("Player utility value: "+cruiserBaseUtility);
                return cruiserBaseUtility;
            }
        }

    }
}
