using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Smoke;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Effects.Smoke;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.UI.Sound.Players;
using Unity.Netcode;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables
{
    public abstract class PvPBuildable<TPvPActivationArgs> : PvPTarget, IPvPBuildable, IPoolable<TPvPActivationArgs>
        where TPvPActivationArgs : PvPBuildableActivationArgs
    {
        private float _cumulativeBuildProgressInDroneS;
        protected float _buildTimeInDroneSeconds;
        private ClickHandler _clickHandler;
        // Keep reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        protected PvPSmokeInitialiser _smokeInitialiser;
#pragma warning restore CS0414  // Variable is assigned but never used
        // All buildables are wrapped by a UnitWrapper or BuildingWrapper, which contains
        // both the target and the health bar.
        public GameObject _parent;
        private IDroneFeedback _droneFeedback;

        protected PvPUIManager _uiManager;
        protected IDroneConsumerProvider _droneConsumerProvider;
        protected AircraftProvider _aircraftProvider;
        protected PvPCruiserSpecificFactories _cruiserSpecificFactories;
        // Boost resulting from global cruiser bonuses
        protected IBoostableGroup _buildRateBoostableGroup;
        protected IBoostableGroup _healthBoostableGroup;
        // Boost resulting from adjacent local boosters
        protected IBoostableGroup _localBoosterBoostableGroup;
        protected PvPBuildableProgressController _buildableProgress;

        public string stringKeyName;
        public string keyName { get; set; }
        public int numOfDronesRequired;
        public float buildTimeInS;

        private AudioClipWrapper _deathSound;
        private float maxHealthBase;
        [Header("Sounds")]
        public AudioClip deathSound;

        private const float MAX_BUILD_PROGRESS = 1;

        private PvPBuildableState _buildState;
        #region Properties
        public PvPBuildableState BuildableState
        {
            get
            {
                return _buildState;
            }
            set
            {
                _buildState = value;
                if (IsServer)
                    OnBuildableStateValueChanged(value);
            }
        }
        public float BuildProgress { get; set; }
        public int NumOfDronesRequired => numOfDronesRequired;
        public float BuildTimeInS => buildTimeInS;
        public IBoostable BuildProgressBoostable { get; private set; }
        public IBoostable HealthBoostable { get; private set; }
        public override Vector2 Size => _buildableProgress.FillableImage.sprite.bounds.size;
        public float CostInDroneS => NumOfDronesRequired * BuildTimeInS;
        protected virtual PrioritisedSoundKey ConstructionCompletedSoundKey => null;
        public IPvPCruiser ParentCruiser { get; set; }
        public IPvPCruiser EnemyCruiser { get; private set; }
        protected virtual bool ShowSmokeWhenDestroyed => false;
        public string PrefabName => _parent.name.Replace("(Clone)", "").Trim();

        private PvPHealthBarController _healthBar;
        public PvPHealthBarController HealthBar => _healthBar;

        private IList<IDamageCapability> _damageCapabilities;
        public ReadOnlyCollection<IDamageCapability> DamageCapabilities { get; private set; }

        private IDroneConsumer _droneConsumer;
        public IDroneConsumer DroneConsumer
        {
            get { return _droneConsumer; }
            protected set
            {
                if (_droneConsumer != null)
                {
                    _droneConsumer.DroneNumChanged -= DroneConsumer_DroneNumChanged;
                    _droneConsumer.DroneStateChanged -= DroneConsumer_DroneStateChanged;
                }

                _droneConsumer = value;
                ToggleDroneConsumerFocusCommand.EmitCanExecuteChanged();

                if (_droneConsumer != null)
                {
                    _droneConsumer.DroneNumChanged += DroneConsumer_DroneNumChanged;
                    _droneConsumer.DroneStateChanged += DroneConsumer_DroneStateChanged;
                }

                ShareIsDroneConsumerFocusableValueWithClient(DroneConsumer != null);
            }
        }

        private IList<SpriteRenderer> _inGameRenderers;
        private IList<SpriteRenderer> InGameRenderers
        {
            // Lazily initialise so that the StaticInitialise() (constructor
            // equivalent) of this class and all child classes has completed.
            // Ie, cannot call GetInGameRenderers() from Buildable.StaticInitialise()
            // because child implementations of GetInGameRenderers() may rely
            // on their StaticInitialise() having run already.
            get
            {
                if (_inGameRenderers == null)
                {
                    _inGameRenderers = GetInGameRenderers();
                }
                return _inGameRenderers;
            }
        }

        private bool IsDroneConsumerFocusable => DroneConsumer != null;
        protected bool IsDroneConsumerFocusable_PvPClient;
        public Command ToggleDroneConsumerFocusCommand { get; private set; }
        public bool IsInitialised => BuildProgressBoostable != null;


        protected virtual ObservableCollection<IBoostProvider> TurretFireRateBoostProviders
        {
            get
            {
                return _cruiserSpecificFactories.GlobalBoostProviders.DummyBoostProviders;
            }
        }

        public override Color Color
        {
            set
            {
                foreach (SpriteRenderer renderer in InGameRenderers)
                {
                    if (renderer == null)
                        return;
                    renderer.color = value;
                }
                if (_buildableProgress == null)
                    return;
                _buildableProgress.FillableImage.color = value;
                _buildableProgress.OutlineImage.color = value;
            }
        }

        #region IComparableItem
        /// <summary>
        /// Allow different sprites to be used for the UI, so:
        /// + Can user higher res sprites
        /// + Can chooser nicer sprites for planes, which have multiple sprites
        /// </summary>
        public Sprite uiFriendlySprite;
        public Sprite Sprite => uiFriendlySprite != null ? uiFriendlySprite : _buildableProgress.FillableImage.sprite;

        public string Description { get; protected set; }
        public string Name { get; protected set; }
        #endregion IComparableItem
        #endregion Properties

        public event EventHandler StartedConstruction;
        public event EventHandler CompletedBuildable;
        public event EventHandler<PvPBuildProgressEventArgs> BuildableProgress;
        public event EventHandler<DroneNumChangedEventArgs> DroneNumChanged;
        public event EventHandler Clicked;
        public event EventHandler Deactivated;


        protected virtual void OnBuildableStateValueChanged(PvPBuildableState state) { }

        void ShareIsDroneConsumerFocusableValueWithClient(bool isFocusable)
        {
            OnShareIsDroneConsumerFocusableValueWithClientRpc(isFocusable);
        }

        [ClientRpc]
        private void OnShareIsDroneConsumerFocusableValueWithClientRpc(bool isFocusable)
        {
            if (!IsHost)
                IsDroneConsumerFocusable_PvPClient = isFocusable;
        }

        protected override void CallRpc_ProgressControllerVisible(bool isEnabled)
        {
            if (IsClient)
                // in some case, smoke strong is not removed from scene in client side, so force stop it when boat start to build.
                _smokeInitialiser.gameObject.GetComponent<Smoke>()._particleSystem.Clear();
        }

        private void OnHealthbarOffsetChanged()
        {
            if (IsServer)
                CallRpc_SetHealthbarOffset(_healthBar.Offset);
        }

        void CallRpc_SetHealthbarOffset(Vector2 offset)
        {
            OnSetHealthbarOffsetClientRpc(offset);
        }

        [ClientRpc]
        void OnSetHealthbarOffsetClientRpc(Vector2 offset)
        {
            if (!IsHost)
                HealthBar.Offset = offset;
        }

        public virtual void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise();
            keyName = stringKeyName;
            Helper.AssertIsNotNull(parent, healthBar);

            _parent = parent;
            _healthBar = healthBar;
            maxHealthBase = MaxHealth;
            _healthBar.OffsetChanged += OnHealthbarOffsetChanged;

            _buildableProgress = gameObject.GetComponentInChildren<PvPBuildableProgressController>(includeInactive: true);
            Assert.IsNotNull(_buildableProgress);
            _buildableProgress.Initialise();

            ToggleDroneConsumerFocusCommand = new Command(ToggleDroneConsumerFocusCommandExecute, () => IsServer ? IsDroneConsumerFocusable : IsDroneConsumerFocusable_PvPClient);

            ClickHandlerWrapper clickHandlerWrapper = GetComponent<ClickHandlerWrapper>();
            Assert.IsNotNull(clickHandlerWrapper);
            _clickHandler = clickHandlerWrapper.GetClickHandler();

            _damageCapabilities = new List<IDamageCapability>();
            DamageCapabilities = new ReadOnlyCollection<IDamageCapability>(_damageCapabilities);
            _smokeInitialiser = GetComponentInChildren<PvPSmokeInitialiser>(includeInactive: true);
            Assert.IsNotNull(_smokeInitialiser);
            Assert.IsNotNull(deathSound);
            _deathSound = new AudioClipWrapper(deathSound);
            //  PvP_HealthbarOffset.OnValueChanged += OnPvPHealthBarOffsetChanged;
        }

        protected void AddDamageStats(IDamageCapability statsToAdd)
        {
            Assert.IsFalse(_damageCapabilities.Contains(statsToAdd));
            _damageCapabilities.Add(statsToAdd);
            UpdateAttackCapabilities();
        }

        private void UpdateAttackCapabilities()
        {
            foreach (IDamageCapability damageStat in _damageCapabilities)
                foreach (TargetType attackCapability in damageStat.AttackCapabilities)
                    AddAttackCapability(attackCapability);
        }
    
        protected override List<SpriteRenderer> GetInGameRenderers()
        {
            SpriteRenderer mainRenderer = GetComponent<SpriteRenderer>();
            //Assert.IsNotNull(mainRenderer);
            if (mainRenderer == null)
            {
                return new List<SpriteRenderer>() { };
            }
            return new List<SpriteRenderer>() { mainRenderer };
        }

        /// <summary>
        /// Called only once, when an object is first instantiated.
        /// </summary>
        public virtual void Initialise()
        {
            Logging.Log(Tags.BUILDABLE, this);

            Assert.IsNotNull(_parent, "Must call StaticInitialise() before Initialise(...)");

            // _uiManager = uiManager;
            _buildTimeInDroneSeconds = numOfDronesRequired * buildTimeInS;
            HealthGainPerDroneS = maxHealth / _buildTimeInDroneSeconds;
            BuildProgressBoostable = new Boostable(1);
            HealthBoostable = new Boostable(1);

            _clickHandler.SingleClick += ClickHandler_SingleClick;
            _clickHandler.DoubleClick += ClickHandler_DoubleClick;

            _healthBar.Initialise(this, followDamagable: true);

            Logging.Log(Tags.BUILDABLE, $"{this}:  _parent.SetActive(false);");
            _parent.SetActive(false);
            if (_parent.GetComponent<PvPBuildingWrapper>() is not null && IsServer)
            {
                _parent.GetComponent<PvPBuildingWrapper>().IsVisible = false;
            }

            if (_parent.GetComponent<PvPUnitWrapper>() is not null && IsServer)
            {
                _parent.GetComponent<PvPUnitWrapper>().IsVisible = false;
            }
        }

        public virtual void Initialise(PvPUIManager uiManager)
        {
            Logging.Log(Tags.BUILDABLE, this);

            Assert.IsNotNull(_parent, "Must call StaticInitialise() before Initialise(...)");
            _uiManager = uiManager;
            _buildTimeInDroneSeconds = numOfDronesRequired * buildTimeInS;
            HealthGainPerDroneS = maxHealth / _buildTimeInDroneSeconds;
            BuildProgressBoostable = new Boostable(1);
            HealthBoostable = new Boostable(1);
            if (!IsHost)
            {
                _clickHandler.SingleClick += ClickHandler_SingleClick;
                _clickHandler.DoubleClick += ClickHandler_DoubleClick;
            }
            _healthBar.Initialise(this, followDamagable: true);
        }

        public virtual void Activate(IPvPCruiser parentCruiser, IPvPCruiser enemyCruiser, PvPCruiserSpecificFactories cruiserSpecificFactories)
        {
            _parent.SetActive(true);
            if (_parent.GetComponent<PvPBuildingWrapper>() is not null && IsServer)
            {
                _parent.GetComponent<PvPBuildingWrapper>().IsVisible = true;
            }
            if (_parent.GetComponent<PvPUnitWrapper>() is not null && IsServer)
            {
                _parent.GetComponent<PvPUnitWrapper>().IsVisible = true;
            }
            ParentCruiser = parentCruiser;
            EnemyCruiser = enemyCruiser;
            _cruiserSpecificFactories = cruiserSpecificFactories;
            _droneConsumerProvider = ParentCruiser.DroneConsumerProvider;
            Faction = ParentCruiser.Faction;
            CallRpc_SyncFaction(Faction);
            _aircraftProvider = _cruiserSpecificFactories.AircraftProvider;
            _localBoosterBoostableGroup = new BoostableGroup();
            _buildRateBoostableGroup = CreateBuildRateBoostableGroup(_cruiserSpecificFactories.GlobalBoostProviders, BuildProgressBoostable);
            _healthBoostableGroup = CreateHealthBoostableGroup(_cruiserSpecificFactories.GlobalBoostProviders, HealthBoostable);
        }

        /// <summary>
        /// Called every time an object is taken from an object pool and activated.  Can happen
        /// multiple times for buildables as they are recycled.
        /// </summary>
        public virtual void Activate(TPvPActivationArgs activationArgs)
        {
            Logging.Log(Tags.BUILDABLE, $"{this}:  _parent.SetActive(true);");
            Assert.IsNotNull(activationArgs);

            Assert.IsFalse(_parent.activeSelf);
            _parent.SetActive(true);
            if (_parent.GetComponent<PvPBuildingWrapper>() is not null && IsServer)
            {
                _parent.GetComponent<PvPBuildingWrapper>().IsVisible = true;
            }
            if (_parent.GetComponent<PvPUnitWrapper>() is not null && IsServer)
            {
                _parent.GetComponent<PvPUnitWrapper>().IsVisible = true;
            }
            ParentCruiser = activationArgs.ParentCruiser;
            _droneConsumerProvider = ParentCruiser.DroneConsumerProvider;
            Faction = ParentCruiser.Faction;
            CallRpc_SyncFaction(Faction);
            EnemyCruiser = activationArgs.EnemyCruiser;

            _cruiserSpecificFactories = activationArgs.CruiserSpecificFactories;
            _aircraftProvider = activationArgs.CruiserSpecificFactories.AircraftProvider;

            BuildableState = PvPBuildableState.NotStarted;
            _cumulativeBuildProgressInDroneS = 0;

            _localBoosterBoostableGroup = new BoostableGroup();
            
            // Apply specialized buildable modifiers BEFORE creating boost groups
            ApplySpecializedModifiers(_cruiserSpecificFactories.GlobalBoostProviders);
            
            _buildRateBoostableGroup = CreateBuildRateBoostableGroup(_cruiserSpecificFactories.GlobalBoostProviders, BuildProgressBoostable);
            _healthBoostableGroup = CreateHealthBoostableGroup(_cruiserSpecificFactories.GlobalBoostProviders, HealthBoostable);
            _healthBoostableGroup.BoostChanged += HealthBoostChanged;
            HealthBoostChanged(this, EventArgs.Empty);
        }

        private void HealthBoostChanged(object sender, EventArgs e)
        {
            maxHealth = maxHealthBase * HealthBoostable.BoostMultiplier;
            _healthTracker.OverrideMaxHealth(maxHealth);
            HealthBar.OverrideHealth(this);
        }

        public virtual void Activate_PvPClient()
        {
            BuildableState = PvPBuildableState.NotStarted;
            _cumulativeBuildProgressInDroneS = 0;
        }

        public void Activate(TPvPActivationArgs activationArgs, Faction faction) { }

        private IBoostableGroup CreateHealthBoostableGroup(GlobalBoostProviders globalBoostProviders, IBoostable healthBoostable)
        {
            IBoostableGroup healthBoostableGroup = new BoostableGroup();
            healthBoostableGroup.AddBoostable(healthBoostable);

            IList<ObservableCollection<IBoostProvider>> healthBoostProvidersList = new List<ObservableCollection<IBoostProvider>>();
            AddHealthBoostProviders(globalBoostProviders, healthBoostProvidersList);

            foreach (ObservableCollection<IBoostProvider> healthBoostProviders in healthBoostProvidersList)
            {
                healthBoostableGroup.AddBoostProvidersList(healthBoostProviders);
            }

            return healthBoostableGroup;
        }

        private IBoostableGroup CreateBuildRateBoostableGroup(GlobalBoostProviders globalBoostProviders, IBoostable buildProgressBoostable)
        {
            IBoostableGroup buildRateBoostableGroup = new BoostableGroup();
            buildRateBoostableGroup.AddBoostable(buildProgressBoostable);

            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList = new List<ObservableCollection<IBoostProvider>>();
            AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);

            foreach (ObservableCollection<IBoostProvider> buildRateBoostProviders in buildRateBoostProvidersList)
            {
                buildRateBoostableGroup.AddBoostProvidersList(buildRateBoostProviders);
            }

            return buildRateBoostableGroup;
        }

        protected virtual void AddHealthBoostProviders(
            GlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> healthBoostProvidersList)
        {
            Logging.Log(Tags.BOOST, this);
        }

        /// <summary>
        /// To allow multiple boost provider sources.  Eg, for the ShieldGenerator:
        /// + Tacticals => Boost from Trident
        /// + Shields   => Boost from Raptor
        /// </summary>
        protected virtual void AddBuildRateBoostProviders(
            GlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            Logging.Log(Tags.BOOST, this);
        }

        private void ApplySpecializedModifiers(GlobalBoostProviders globalBoostProviders)
        {
            if (globalBoostProviders.SpecializedBuildableBoosts.TryGetValue(PrefabName, out var modifiers))
            {
                Logging.Log(Tags.BOOST, $"Applying specialized modifiers to {PrefabName}");
                
                // Apply drone requirement override
                if (modifiers.droneRequirementOverride > 0)
                {
                    numOfDronesRequired = modifiers.droneRequirementOverride;
                }
                
                // Apply build time multiplier
                buildTimeInS *= modifiers.buildTimeMultiplier;
                
                // Apply health multiplier (PvP buildables don't have maxHealthBase)
                maxHealth *= modifiers.healthMultiplier;
                
                // Recalculate derived values with modified stats
                _buildTimeInDroneSeconds = numOfDronesRequired * buildTimeInS;
                HealthGainPerDroneS = maxHealth / _buildTimeInDroneSeconds;
            }
        }

        private void ClickHandler_SingleClick(object sender, EventArgs e)
        {
            Logging.Log(Tags.BUILDABLE, this);

            OnSingleClick();
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        protected abstract void OnSingleClick();

        private void ClickHandler_DoubleClick(object sender, EventArgs e)
        {
            Logging.Log(Tags.BUILDABLE, this);
            OnDoubleClick();
        }

        protected virtual void OnDoubleClick() { }

        private void DroneConsumer_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            DroneNumChanged?.Invoke(this, e);
        }

        private void DroneConsumer_DroneStateChanged(object sender, DroneStateChangedEventArgs e)
        {
            if (BuildableState != PvPBuildableState.Completed)
            {
                if (e.OldState == DroneConsumerState.Idle)
                {
                    BuildableState = PvPBuildableState.InProgress;
                }
                else if (e.NewState == DroneConsumerState.Idle)
                {
                    BuildableState = PvPBuildableState.Paused;
                }
            }
        }

        public virtual void StartConstruction()
        {
            Logging.Log(Tags.BUILDABLE, this);

            _healthTracker.SetMinHealth();

            SetupDroneConsumer(numOfDronesRequired, showDroneFeedback: true);

            EnableRenderers(false);

            if (DroneConsumer.State != DroneConsumerState.Idle)
            {
                BuildableState = PvPBuildableState.InProgress;
            }

            StartedConstruction?.Invoke(this, EventArgs.Empty);
            CallRpc_ProgressControllerVisible(true);
        }

        // PERF  Doesn't need to be every update :)
        public void Update()
        {
            if (IsServer)
            {
                if (BuildableState == PvPBuildableState.InProgress)
                {
                    Assert.IsTrue(DroneConsumer.State != DroneConsumerState.Idle);

                    // Find build progress
                    float buildProgressInDroneS = ParentCruiser.BuildProgressCalculator.CalculateBuildProgressInDroneS(this, _time.DeltaTime);
                    _cumulativeBuildProgressInDroneS += buildProgressInDroneS;

                    BuildProgress = _cumulativeBuildProgressInDroneS / _buildTimeInDroneSeconds;

                    if (BuildProgress > MAX_BUILD_PROGRESS)
                    {
                        BuildProgress = MAX_BUILD_PROGRESS;
                    }

                    // Increase health with build progress
                    float buildProgressIncrement = buildProgressInDroneS / _buildTimeInDroneSeconds;
                    _healthTracker.AddHealth(buildProgressIncrement * MaxHealth);

                    BuildableProgress?.Invoke(this, new PvPBuildProgressEventArgs(this));
                    //    OnBuildableProgressEvent();
                    if (_cumulativeBuildProgressInDroneS >= _buildTimeInDroneSeconds)
                    {
                        OnBuildableCompleted();
                    }
                }
                OnUpdate();
            }
            if (IsClient)
            {
                _buildableProgress.FillableImage.fillAmount = BuildProgress;
                BuildableProgress?.Invoke(this, new PvPBuildProgressEventArgs(this));
            }
        }

        protected virtual void OnBuildableProgressEvent()
        {
            if (IsClient)
                BuildableProgress?.Invoke(this, new PvPBuildProgressEventArgs(this));
        }

        protected virtual void OnCompletedBuildableEvent()
        {
            if (IsClient)
                CompletedBuildable?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnUpdate() { }

        protected virtual void OnBuildableCompleted()
        {
            Logging.Log(Tags.BUILDABLE, this);

            CleanUpDroneConsumer();

            EnableRenderers(true);
            BuildableState = PvPBuildableState.Completed;
            if (ConstructionCompletedSoundKey != null)
            {
                PlayBuildableConstructionCompletedSound();
            }
            CompletedBuildable?.Invoke(this, EventArgs.Empty);
            OnCompletedBuildableEvent();
            CallRpc_ProgressControllerVisible(false);
            RepairCommand.EmitCanExecuteChanged();
            if (Faction == Faction.Blues)
                PvPBattleSceneGodTunnel.AddAllBuildablesOfLeftPlayer(TargetType, PvPBattleSceneGodTunnel.difficultyDestructionScoreMultiplier * numOfDronesRequired * buildTimeInS);
            else
                PvPBattleSceneGodTunnel.AddAllBuildablesOfRightPlayer(TargetType, PvPBattleSceneGodTunnel.difficultyDestructionScoreMultiplier * numOfDronesRequired * buildTimeInS);
        }

        protected virtual void OnBuildableCompleted_PvPClient()
        {
            BuildableState = PvPBuildableState.Completed;
            CompletedBuildable?.Invoke(this, EventArgs.Empty);
            OnCompletedBuildableEvent();
            CallRpc_ProgressControllerVisible(false);
            RepairCommand.EmitCanExecuteChanged();
            ToggleDroneConsumerFocusCommand.EmitCanExecuteChanged();
            if (Faction == Faction.Blues)
                PvPBattleSceneGodTunnel.AddAllBuildablesOfLeftPlayer(TargetType, PvPBattleSceneGodTunnel.difficultyDestructionScoreMultiplier * numOfDronesRequired * buildTimeInS);
            else
                PvPBattleSceneGodTunnel.AddAllBuildablesOfRightPlayer(TargetType, PvPBattleSceneGodTunnel.difficultyDestructionScoreMultiplier * numOfDronesRequired * buildTimeInS);
        }

        protected virtual void PlayBuildableConstructionCompletedSound()
        {
            if (ConstructionCompletedSoundKey != null)
            {
                if (IsClient && IsOwner)
                    PvPFactoryProvider.Sound.IPrioritisedSoundPlayer.PlaySound(ConstructionCompletedSoundKey);
            }
        }

        private void SetRenderersEnabled(bool enabled)
        {
            Logging.Log(Tags.BUILDING, $"Renderer count: {InGameRenderers.Count}  enabled: {enabled}");
            foreach (SpriteRenderer r in InGameRenderers)
                if (r != null) 
                    r.enabled = enabled;
        }

        private void EnableRenderers(bool enabled)
        {
            if (IsClient)
                SetRenderersEnabled(enabled);
            if (IsServer)
                EnableRenderersClientRpc(enabled);
        }

        [ClientRpc]
        private void EnableRenderersClientRpc(bool enabled)
        {
            if (IsHost)
                return;
            SetRenderersEnabled(enabled);
        }

        protected override void InternalDestroy()
        {
            Deactivate();
        }

        protected virtual void Deactivate()
        {
            //   Logging.Log(Tags.BUILDABLE, $"{this}:  _parent.SetActive(false);");
            if (!IsHost)
                return;
            Assert.IsTrue(_parent.activeSelf);
            _parent.SetActive(false);
            if (_parent.GetComponent<PvPBuildingWrapper>() is not null && IsServer)
            {
                _parent.GetComponent<PvPBuildingWrapper>().IsVisible = false;
            }
            if (_parent.GetComponent<PvPUnitWrapper>() is not null && IsServer)
            {
                _parent.GetComponent<PvPUnitWrapper>().IsVisible = false;
            }
            Deactivated?.Invoke(this, EventArgs.Empty);
            //   Invoke("iDestroyParentGameObject", 1f);
        }
        private void iDestroyParentGameObject()
        {
            Destroy(_parent.gameObject);
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            if (DroneConsumer != null)
            {
                CleanUpDroneConsumer();
            }
            _healthBar.OffsetChanged -= OnHealthbarOffsetChanged;

            _localBoosterBoostableGroup.CleanUp();
            _buildRateBoostableGroup.CleanUp();
            _healthBoostableGroup.CleanUp();

            CallRpc_PlayDeathSound();

            if (Faction == Faction.Reds)
            {
                int val = (int)_cumulativeBuildProgressInDroneS;
                PvPBattleSceneGodServer.AddDeadBuildable_Left(TargetType, val);
            }
            else
            {
                int val = (int)_cumulativeBuildProgressInDroneS;
                PvPBattleSceneGodServer.AddDeadBuildable_Right(TargetType, val);
            }
        }

        protected void SetupDroneConsumer(int numOfDrones, bool showDroneFeedback)
        {
            Logging.Log(Tags.BUILDABLE, $"{this}  numOfDrones: {numOfDrones}");

            Assert.IsNull(DroneConsumer);
            DroneConsumer = _droneConsumerProvider.RequestDroneConsumer(numOfDrones);
            _droneFeedback
                = showDroneFeedback ?
                    _cruiserSpecificFactories.DroneFeedbackFactory.CreateFeedback(DroneConsumer, DroneAreaPosition, DroneAreaSize) :
                    _cruiserSpecificFactories.DroneFeedbackFactory.CreateDummyFeedback();
            _droneConsumerProvider.ActivateDroneConsumer(DroneConsumer);

            Logging.Log(Tags.BUILDABLE, $"{Name}   Want: {numOfDrones}  Got: {DroneConsumer.NumOfDrones}");
        }

        protected void CleanUpDroneConsumer()
        {
            Logging.Log(Tags.BUILDABLE, this);

            Assert.IsNotNull(_droneFeedback);
            _droneFeedback.DisposeManagedState();
            _droneFeedback = null;

            Assert.IsNotNull(DroneConsumer);
            _droneConsumerProvider.ReleaseDroneConsumer(DroneConsumer);
            DroneConsumer = null;
        }

        protected override bool CanRepairCommandExecute()
        {
            return
                base.CanRepairCommandExecute()
                && BuildableState == PvPBuildableState.Completed;
        }

        protected virtual void ToggleDroneConsumerFocusCommandExecute()
        {
            if (IsServer)
                ParentCruiser.DroneFocuser.ToggleDroneConsumerFocus(DroneConsumer, isTriggeredByPlayer: true);
            else
                CallRpc_ToggleDroneConsumerFocusCommandExecute();
        }


        protected virtual void CallRpc_ToggleDroneConsumerFocusCommandExecute()
        {
            if (IsServer)
                ToggleDroneConsumerFocusCommandExecute();
        }

        protected virtual void CallRpc_PlayDeathSound()
        {
            if (IsClient)
            {
                SoundPlayer.PlaySound(_deathSound.AudioClip, transform.position);
                // in some case, smoke strong is not removed from scene in client side, so force stop it when boat destroyed.
                //   _smokeInitialiser.gameObject.GetComponent<PvPSmoke>()._particleSystem.Clear();

                if (Faction == Faction.Reds)
                {
                    if (TargetType == TargetType.Ships)
                    {
                        if (UnityEngine.Random.Range(0, 2) == 0) PvPCaptainExoHUDController.Instance.DoLeftHappy(); else PvPCaptainExoHUDController.Instance.DoLeftTaunt();
                        PvPCaptainExoHUDController.Instance.DoRightAngry();
                        /*                        if (UnityEngine.Random.Range(0, 5) == 1)
                                                    PvPHeckleMessageManager.Instance.ShowAIBotHeckle();*/
                        return;
                    }
                    if (TargetType == TargetType.Buildings)
                    {
                        if (UnityEngine.Random.Range(0, 2) == 0) PvPCaptainExoHUDController.Instance.DoLeftHappy(); else PvPCaptainExoHUDController.Instance.DoLeftTaunt();
                        PvPCaptainExoHUDController.Instance.DoRightAngry();
                        /*                        if (UnityEngine.Random.Range(0, 5) == 1)
                                                    PvPHeckleMessageManager.Instance.ShowAIBotHeckle();*/
                        return;
                    }
                }
                else
                {
                    if (TargetType == TargetType.Ships)
                    {
                        if (UnityEngine.Random.Range(0, 2) == 0) PvPCaptainExoHUDController.Instance.DoRightHappy(); else PvPCaptainExoHUDController.Instance.DoRightTaunt();

                        /*                        if (UnityEngine.Random.Range(0, 3) == 1)
                                                    PvPHeckleMessageManager.Instance.ShowAIBotHeckle();*/
                        PvPCaptainExoHUDController.Instance.DoLeftAngry();
                        return;
                    }
                    if (TargetType == TargetType.Buildings)
                    {
                        if (UnityEngine.Random.Range(0, 2) == 0) PvPCaptainExoHUDController.Instance.DoRightHappy(); else PvPCaptainExoHUDController.Instance.DoRightTaunt();
                        /*                        if (UnityEngine.Random.Range(0, 3) == 1)
                                                    PvPHeckleMessageManager.Instance.ShowAIBotHeckle();*/
                        PvPCaptainExoHUDController.Instance.DoLeftAngry();
                        return;
                    }
                }
            }

        }

        protected virtual void CallRpc_SyncFaction(Faction faction)
        {

        }
    }
}
