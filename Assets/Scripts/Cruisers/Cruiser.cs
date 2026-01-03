using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    public class Cruiser : Target, ICruiser, IComparableItem
    {
        protected UIManager _uiManager;
        protected ICruiser _enemyCruiser;
        protected SpriteRenderer _renderer;
        protected Collider2D _collider;
        protected ICruiserHelper _helper;
        public SlotWrapperController SlotWrapperController;
        protected ClickHandler _clickHandler;
        protected IDoubleClickHandler<IBuilding> _buildingDoubleClickHandler;
        protected IDoubleClickHandler<ICruiser> _cruiserDoubleClickHandler;
        protected AudioClipWrapper _selectedSound;
        // Keep reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        protected IManagedDisposable _fogOfWarManager, _unitReadySignal, _droneFeedbackSound;
#pragma warning restore CS0414  // Variable is assigned but never used
        protected SettingsManager settingsManager;

        // CruiserSection array: supports 1-N sections (1 for single-section, N for multi-section)
        protected CruiserSection[] _hulls;
        public CruiserSection[] Hulls => _hulls;  // Public accessor for external code

        public string stringKeyBase;
        public int numOfDrones = 4;
        public float yAdjustmentInM;
        public Vector2 trashTalkScreenPosition = new Vector2(480, -28);
        public HullType hullType;
        public bool startsWithFogOfWar;

        [Tooltip("GameObjects that persist in the scene after this cruiser is destroyed (e.g., CivBuildings)")]
        public GameObject[] persistentObjects;

        [Header("Additional Colliders")]
        [Tooltip("Enable to use additional PolygonCollider2D components as part of this cruiser's hitbox. Useful for animated cruisers with backup enemies that appear.")]
        public bool useAdditionalColliders = false;

        [Tooltip("Additional PolygonCollider2D components that should be treated as part of this cruiser's hitbox. These can be on child GameObjects.")]
        public PolygonCollider2D[] additionalColliders;

        // ITarget
        public override TargetType TargetType => TargetType.Cruiser;
        public override Color Color {
            set {
                // Apply color to all sections if they exist
                if (_hulls != null)
                {
                    foreach (var section in _hulls)
                    {
                        if (section != null)
                        {
                            section.Color = value;
                        }
                    }
                }
                // Also apply to root renderer if it exists (for single-section legacy)
                if (_renderer != null)
                {
                    _renderer.color = value;
                }
            }
        }
        public override Vector2 Size
        {
            get
            {
                // If we have sections, use primary section's size
                if (_hulls != null && _hulls.Length > 0 && _hulls[0] != null)
                {
                    return _hulls[0].Size;
                }

                // Fallback to single-section logic
                if (!useAdditionalColliders || additionalColliders == null || additionalColliders.Length == 0)
                {
                    return _collider.bounds.size;
                }

                // Combine bounds from main collider and all additional colliders
                Bounds combinedBounds = _collider.bounds;
                foreach (var extraCollider in additionalColliders)
                {
                    if (extraCollider != null)
                    {
                        combinedBounds.Encapsulate(extraCollider.bounds);
                    }
                }
                return combinedBounds.size;
            }
        }
        public override Vector2 DroneAreaPosition => new Vector2(Position.x, Position.y - Size.y / 4);

        protected Vector2 _droneAreaSize;
        public override Vector2 DroneAreaSize => _droneAreaSize;


        // IComparableItem
        public string Description { get; protected set; }
        public string Name { get; protected set; }
        public Sprite Sprite
        {
            get
            {
                // Return primary hull sprite if available
                if (_hulls != null && _hulls.Length > 0 && _hulls[0] != null && _hulls[0].SpriteRenderer != null)
                {
                    return _hulls[0].SpriteRenderer.sprite;
                }
                // Fallback to root renderer
                return _renderer?.sprite;
            }
        }

        // ICruiser
        public IBuildableWrapper<IBuilding> SelectedBuildingPrefab { get; set; }
        public IDroneConsumerProvider DroneConsumerProvider { get; protected set; }
        public Direction Direction { get; protected set; }
        public float YAdjustmentInM => yAdjustmentInM;
        public Vector2 TrashTalkScreenPosition => trashTalkScreenPosition;
        public CruiserSpecificFactories CruiserSpecificFactories { get; protected set; }
        protected FogOfWar _fog;
        public IGameObject Fog => _fog;
        public IRepairManager RepairManager { get; protected set; }
        public int NumOfDrones => numOfDrones;
        public IBuildProgressCalculator BuildProgressCalculator { get; protected set; }
        public bool IsPlayerCruiser => Position.x < 0;
        public CruiserDeathExplosion deathPrefab;
        public CruiserDeathExplosion DeathPrefab => deathPrefab;


        // ICruiserController
        // Override health properties to route through primary section if available
        public new virtual float Health
        {
            get
            {
                // If we have sections, return primary section's health
                if (_hulls != null && _hulls.Length > 0 && _hulls[0] != null)
                {
                    return _hulls[0].Health;
                }
                // Fallback to base class health
                return base.Health;
            }
        }

        public new virtual float MaxHealth
        {
            get
            {
                // If we have sections, return primary section's max health
                if (_hulls != null && _hulls.Length > 0 && _hulls[0] != null)
                {
                    return _hulls[0].MaxHealth;
                }
                // Fallback to base class maxHealth
                return maxHealth;
            }
        }

        public new virtual bool IsDestroyed
        {
            get
            {
                // If we have sections, return primary section's destroyed state
                if (_hulls != null && _hulls.Length > 0 && _hulls[0] != null)
                {
                    return _hulls[0].IsDestroyed;
                }
                // Fallback to base class logic
                return base.IsDestroyed;
            }
        }

        public new virtual bool IsAlive
        {
            get
            {
                // If we have sections, return primary section's alive state
                if (_hulls != null && _hulls.Length > 0 && _hulls[0] != null)
                {
                    return !_hulls[0].IsDestroyed;
                }
                // Fallback to base class logic
                return !IsDestroyed;
            }
        }
        public SlotAccessor SlotAccessor { get; protected set; }
        public SlotHighlighter SlotHighlighter { get; protected set; }
        public ISlotNumProvider SlotNumProvider { get; protected set; }
        public DroneManager DroneManager { get; protected set; }
        public IDroneFocuser DroneFocuser { get; protected set; }
        public ICruiserBuildingMonitor BuildingMonitor { get; protected set; }
        public ICruiserUnitMonitor UnitMonitor { get; protected set; }
        public IPopulationLimitMonitor PopulationLimitMonitor { get; protected set; }
        public IUnitTargets UnitTargets { get; protected set; }
        public TargetTracker BlockedShipsTracker { get; protected set; }

        public event EventHandler<BuildingStartedEventArgs> BuildingStarted;
        public event EventHandler<BuildingCompletedEventArgs> BuildingCompleted;
        public event EventHandler<BuildingDestroyedEventArgs> BuildingDestroyed;
        public event EventHandler Clicked;

        // Multi-section events (for secondary section destruction scoring)
        public event EventHandler<CruiserSectionTargetedEventArgs> CruiserSectionTargeted;
        public event EventHandler<CruiserSectionDestroyedEventArgs> SecondaryHullDestroyed;

        public bool isCruiser = true;
        public bool isUsingBodykit = false;

        [Serializable]
        public class BoostStats
        {
            public BoostType boostType;
            public float boostAmount = 1f;
        }

        public List<BoostStats> Boosts;
        List<IBoostProvider> BoostsProvided = new List<IBoostProvider>();

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            Assert.IsNotNull(deathPrefab, $"Death prefab for Cruiser {name} is null");

            // Renderer is optional - single-section cruisers have it on this object,
            // multi-section cruisers have it on CruiserSection children
            _renderer = GetComponent<SpriteRenderer>();

            _collider = GetComponent<Collider2D>();
            Assert.IsNotNull(_collider);

            // Validate additional colliders if enabled
            if (useAdditionalColliders)
            {
                if (additionalColliders == null || additionalColliders.Length == 0)
                {
                    Debug.LogWarning($"[Cruiser] {name}: useAdditionalColliders is enabled but additionalColliders array is empty or null. Additional colliders will be ignored.");
                }
                else
                {
                    int nullCount = 0;
                    foreach (var collider in additionalColliders)
                    {
                        if (collider == null)
                            nullCount++;
                    }
                    if (nullCount > 0)
                    {
                        Debug.LogWarning($"[Cruiser] {name}: {nullCount} null collider(s) found in additionalColliders array. They will be ignored.");
                    }
                }
            }

            SlotWrapperController = GetComponentInChildren<SlotWrapperController>(includeInactive: true);
            Assert.IsNotNull(SlotWrapperController);
            SlotWrapperController.StaticInitialise();
            SlotNumProvider = SlotWrapperController;

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

            // Activate starting fog if cruiser has this perk (AFTER fog initialization)
            if (args.FogOfWarManager is FogOfWarManager pveManager)
            {
                pveManager.ActivateStartingFog();
            }

            SlotAccessor = SlotWrapperController.Initialise(this);
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

            if (Boosts != null)
                foreach (BoostStats boost in Boosts)
                {
                    IBoostProvider boostProvider = new BoostProvider(boost.boostAmount);
                    BoostsProvided.Add(boostProvider);
                    CruiserSpecificFactories.GlobalBoostProviders.BoostTypeToBoostProvider(boost.boostType).Add(boostProvider);
                }

        }

        public void AddBoost(BoostStats boost)
        {
            Boosts.Add(boost);
            IBoostProvider boostProvider = new BoostProvider(boost.boostAmount);
            BoostsProvided.Add(boostProvider);
            CruiserSpecificFactories.GlobalBoostProviders.BoostTypeToBoostProvider(boost.boostType).Add(boostProvider);
        }

        public void RemoveBoost(BoostStats boost)
        {
            for(int i = 0; i < Boosts.Count; i++)
                if(Boosts[i].boostType == boost.boostType)
                {
                    Boosts.RemoveAt(i);
                    CruiserSpecificFactories.GlobalBoostProviders.BoostTypeToBoostProvider(boost.boostType).Remove(BoostsProvided[i]);
                    BoostsProvided.RemoveAt(i);
                }
        }

        // ============== CruiserSection Event Callbacks ==============
        // Called by CruiserSection when clicked - can be overridden for multi-section behavior
        public virtual void OnHullClicked(CruiserSection section)
        {
            // Default: forward to standard click behavior
            _clickHandler_SingleClick(null, EventArgs.Empty);
        }

        // Called by CruiserSection when double-clicked
        public virtual void OnHullDoubleClicked(CruiserSection section)
        {
            // Default: forward to standard double-click behavior
            _clickHandler_DoubleClick(null, EventArgs.Empty);
        }

        // Called by CruiserSection when triple-clicked
        public virtual void OnHullTripleClicked(CruiserSection section)
        {
            // Default: forward to standard double-click behavior (targeting)
            _clickHandler_DoubleClick(null, EventArgs.Empty);
        }

        // Called by CruiserSection when destroyed
        public virtual void OnHullDestroyed(CruiserSection section)
        {
            // Default: if primary section is destroyed, cruiser is destroyed
            if (section.IsPrimary)
            {
                Destroy();
            }
            else if (_hulls != null && _hulls.Length > 1)
            {
                // Secondary section destruction in multi-section cruiser: award points
                SecondaryHullDestroyed?.Invoke(this, new CruiserSectionDestroyedEventArgs(section));

                // Add partial destruction score for enemy cruisers
                if (Faction == Faction.Reds)
                {
                    BattleSceneGod.AddDeadBuildable(TargetType.Buildings, (int)(section.maxHealth * 0.3f));
                }
            }
        }

        // Setup the section array - called during initialization
        public virtual void SetupHulls(CruiserSection[] sections)
        {
            _hulls = sections;
            if (_hulls != null && _hulls.Length > 0)
            {
                // Set cruiser's max health from primary section
                maxHealth = _hulls[0].maxHealth;
            }
        }

        // ============== End CruiserSection Event Callbacks ==============

        protected virtual void _clickHandler_SingleClick(object sender, EventArgs e)
        {
            Logging.LogMethod(Tags.CRUISER);

            _uiManager.ShowCruiserDetails(this);
            _helper.FocusCameraOnCruiser();
            FactoryProvider.Sound.UISoundPlayer.PlaySound(_selectedSound);

            Clicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void _clickHandler_DoubleClick(object sender, EventArgs e)
        {
            _cruiserDoubleClickHandler.OnDoubleClick(this);
        }

        protected void OnClicked()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        public IBuilding ConstructBuilding(IBuildableWrapper<IBuilding> buildingPrefab, Slot slot,
                                           bool ignoreBuildTime = false,
                                           bool ignoreDroneRequirement = false)
        {
            Logging.Log(Tags.CRUISER, buildingPrefab.Buildable.Name);

            SelectedBuildingPrefab = buildingPrefab;
            return ConstructSelectedBuilding(slot, ignoreBuildTime, ignoreDroneRequirement);
        }

        public IBuilding ConstructSelectedBuilding(Slot slot,
                                                   bool ignoreBuildTime = false,
                                                   bool ignoreDroneRequirement = false)
        {
            Assert.IsNotNull(SelectedBuildingPrefab);
            Assert.AreEqual(SelectedBuildingPrefab.Buildable.SlotSpecification.SlotType, slot.Type);
            IBuilding building = PrefabFactory.CreateBuilding(SelectedBuildingPrefab);
            if (ignoreDroneRequirement)
            {
                building.BuildTimeInS *= building.NumOfDronesRequired;
                building.NumOfDronesRequired = 1;
            }

            /*       SetVariantIcon(building);*/
            building.Activate(
                new BuildingActivationArgs(
                    this,
                    _enemyCruiser,
                    CruiserSpecificFactories,
                    slot,
                    _buildingDoubleClickHandler));
            if (ignoreBuildTime)
                building.BuildProgressBoostable.BoostMultiplier = 1e9f;

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

            if (IsPlayerCruiser && _enemyCruiser != null && _enemyCruiser.IsAlive)
                BattleSceneGod.AddPlayedTime(TargetType.PlayedTime, _time.DeltaTime);
        }

        public void MakeInvincible()
        {
            // Apply to all sections if they exist
            if (_hulls != null)
            {
                foreach (var section in _hulls)
                {
                    section?.MakeInvincible();
                }
            }
            // Also apply to base health tracker
            if (_healthTracker != null)
            {
                _healthTracker.State = HealthTrackerState.Immutable;
            }
        }

        public void MakeDamagable()
        {
            // Apply to all sections if they exist
            if (_hulls != null)
            {
                foreach (var section in _hulls)
                {
                    section?.MakeDamageable();
                }
            }
            // Also apply to base health tracker
            if (_healthTracker != null)
            {
                _healthTracker.State = HealthTrackerState.Mutable;
            }
        }

        public virtual void AdjustStatsByDifficulty(Difficulty AIDifficulty)
        {

        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();


            // Persist specified GameObjects in the scene after cruiser destruction
            if (persistentObjects != null && persistentObjects.Length > 0)
            {
                foreach (GameObject persistentObj in persistentObjects)
                {
                    if (persistentObj != null)
                    {
                        // Unparent to prevent destruction with cruiser
                        persistentObj.transform.SetParent(null);
                    }
                }
            }
            
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

    }
}
