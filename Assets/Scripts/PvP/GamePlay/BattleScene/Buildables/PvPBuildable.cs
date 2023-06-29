using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Smoke;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Commands;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables
{
    public abstract class PvPBuildable<TPvPActivationArgs> : PvPTarget, IPvPBuildable, IPvPPoolable<TPvPActivationArgs>
        where TPvPActivationArgs : PvPBuildableActivationArgs
    {
        private float _cumulativeBuildProgressInDroneS;
        private float _buildTimeInDroneSeconds;
        private IPvPClickHandler _clickHandler;
        // Keep reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private PvPSmokeInitialiser _smokeInitialiser;
#pragma warning restore CS0414  // Variable is assigned but never used
        // All buildables are wrapped by a UnitWrapper or BuildingWrapper, which contains
        // both the target and the health bar.
        private GameObject _parent;
        private IPvPDroneFeedback _droneFeedback;

        protected IPvPUIManager _uiManager;
        protected IPvPDroneConsumerProvider _droneConsumerProvider;
        protected IPvPTargetFactoriesProvider _targetFactories;
        protected IPvPMovementControllerFactory _movementControllerFactory;
        protected IPvPAircraftProvider _aircraftProvider;
        protected IPvPFactoryProvider _factoryProvider;
        protected IPvPCruiserSpecificFactories _cruiserSpecificFactories;
        // Boost resulting from global cruiser bonuses
        protected IPvPBoostableGroup _buildRateBoostableGroup;
        // Boost resulting from adjacent local boosters
        protected IPvPBoostableGroup _localBoosterBoostableGroup;
        protected PvPBuildableProgressController _buildableProgress;

        public string stringKeyName;
        public string keyName { get; set; }
        public int numOfDronesRequired;
        public float buildTimeInS;

        private IPvPAudioClipWrapper _deathSound;
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
        public IPvPBoostable BuildProgressBoostable { get; private set; }
        public override Vector2 Size => _buildableProgress.FillableImage.sprite.bounds.size;
        public float CostInDroneS => NumOfDronesRequired * BuildTimeInS;
        protected virtual PvPPrioritisedSoundKey ConstructionCompletedSoundKey => null;
        public IPvPCruiser ParentCruiser { get; private set; }
        public IPvPCruiser EnemyCruiser { get; private set; }
        protected virtual bool ShowSmokeWhenDestroyed => false;
        public string PrefabName => _parent.name;

        private PvPHealthBarController _healthBar;
        public IPvPHealthBar HealthBar => _healthBar;

        private IList<IPvPDamageCapability> _damageCapabilities;
        public ReadOnlyCollection<IPvPDamageCapability> DamageCapabilities { get; private set; }

        private IPvPDroneConsumer _droneConsumer;
        public IPvPDroneConsumer DroneConsumer
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
        public IPvPCommand ToggleDroneConsumerFocusCommand { get; private set; }
        public bool IsInitialised => BuildProgressBoostable != null;


        protected virtual ObservableCollection<IPvPBoostProvider> TurretFireRateBoostProviders
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
                    renderer.color = value;
                }

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
        public event EventHandler<PvPDroneNumChangedEventArgs> DroneNumChanged;
        public event EventHandler Clicked;
        public event EventHandler Deactivated;


        protected virtual void OnBuildableStateValueChanged(PvPBuildableState state)
        {

        }

        protected virtual void OnValueChangedIsEnableRenderes(bool isEnabled)
        {
            EnableRenderers(isEnabled);
        }

        protected virtual void ShareIsDroneConsumerFocusableValueWithClient(bool isFocusable)
        {

        }

        protected override void CallRpc_ProgressControllerVisible(bool isEnabled)
        {
        }

        private void OnHealthbarOffsetChanged()
        {
            if (IsServer)
                CallRpc_SetHealthbarOffset(_healthBar.Offset);
        }
        protected virtual void CallRpc_SetHealthbarOffset(Vector2 offset)
        {

        }
        public virtual void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(commonStrings);
            keyName = stringKeyName;
            Helper.AssertIsNotNull(parent, healthBar);

            _parent = parent;
            _healthBar = healthBar;
            _healthBar.OffsetChanged += OnHealthbarOffsetChanged;

            _buildableProgress = gameObject.GetComponentInChildren<PvPBuildableProgressController>(includeInactive: true);
            Assert.IsNotNull(_buildableProgress);
            _buildableProgress.Initialise();

            ToggleDroneConsumerFocusCommand = new PvPCommand(ToggleDroneConsumerFocusCommandExecute, () => IsServer ? IsDroneConsumerFocusable : IsDroneConsumerFocusable_PvPClient);

            PvPClickHandlerWrapper clickHandlerWrapper = GetComponent<PvPClickHandlerWrapper>();
            Assert.IsNotNull(clickHandlerWrapper);
            _clickHandler = clickHandlerWrapper.GetClickHandler();

            _damageCapabilities = new List<IPvPDamageCapability>();
            this.DamageCapabilities = new ReadOnlyCollection<IPvPDamageCapability>(_damageCapabilities);

            _smokeInitialiser = GetComponentInChildren<PvPSmokeInitialiser>(includeInactive: true);
            Assert.IsNotNull(_smokeInitialiser);

            Assert.IsNotNull(deathSound);
            _deathSound = new PvPAudioClipWrapper(deathSound);



            //  PvP_HealthbarOffset.OnValueChanged += OnPvPHealthBarOffsetChanged;
        }


        public virtual void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {

            Helper.AssertIsNotNull(parent, healthBar);

            _parent = parent;
            _healthBar = healthBar;

            _buildableProgress = gameObject.GetComponentInChildren<PvPBuildableProgressController>(includeInactive: true);
            Assert.IsNotNull(_buildableProgress);
            _buildableProgress.Initialise();

            ToggleDroneConsumerFocusCommand = new PvPCommand(ToggleDroneConsumerFocusCommandExecute, () => IsServer ? IsDroneConsumerFocusable : IsDroneConsumerFocusable_PvPClient);

            PvPClickHandlerWrapper clickHandlerWrapper = GetComponent<PvPClickHandlerWrapper>();
            Assert.IsNotNull(clickHandlerWrapper);
            _clickHandler = clickHandlerWrapper.GetClickHandler();

            _damageCapabilities = new List<IPvPDamageCapability>();
            this.DamageCapabilities = new ReadOnlyCollection<IPvPDamageCapability>(_damageCapabilities);

            _smokeInitialiser = GetComponentInChildren<PvPSmokeInitialiser>(includeInactive: true);
            Assert.IsNotNull(_smokeInitialiser);

            Assert.IsNotNull(deathSound);
            _deathSound = new PvPAudioClipWrapper(deathSound);

            //  PvP_HealthbarOffset.OnValueChanged += OnPvPHealthBarOffsetChanged;
        }

        protected void AddDamageStats(IPvPDamageCapability statsToAdd)
        {
            Assert.IsFalse(_damageCapabilities.Contains(statsToAdd));
            _damageCapabilities.Add(statsToAdd);
            UpdateAttackCapabilities();
        }

        private void UpdateAttackCapabilities()
        {
            foreach (IPvPDamageCapability damageStat in _damageCapabilities)
            {
                foreach (PvPTargetType attackCapability in damageStat.AttackCapabilities)
                {
                    AddAttackCapability(attackCapability);
                }
            }
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
        public virtual void Initialise(IPvPFactoryProvider factoryProvider)
        {
            Logging.Log(Tags.BUILDABLE, this);

            Assert.IsNotNull(_parent, "Must call StaticInitialise() before Initialise(...)");
            Helper.AssertIsNotNull(factoryProvider);

            // _uiManager = uiManager;
            _factoryProvider = factoryProvider;
            _targetFactories = _factoryProvider.Targets;
            _movementControllerFactory = _factoryProvider.MovementControllerFactory;
            _buildTimeInDroneSeconds = numOfDronesRequired * buildTimeInS;
            HealthGainPerDroneS = maxHealth / _buildTimeInDroneSeconds;
            BuildProgressBoostable = _factoryProvider.BoostFactory.CreateBoostable();

            _clickHandler.SingleClick += ClickHandler_SingleClick;
            _clickHandler.DoubleClick += ClickHandler_DoubleClick;

            _healthBar.Initialise(this, followDamagable: true);

            Logging.Log(Tags.BUILDABLE, $"{this}:  _parent.SetActive(false);");
            _parent.SetActive(false);
            if (_parent.GetComponent<PvPBuildingWrapper>() is not null && IsServer)
            {
                _parent.GetComponent<PvPBuildingWrapper>().IsVisible = false;
            }



        }

        public virtual void Initialise(IPvPFactoryProvider factoryProvider, IPvPUIManager uiManager)
        {
            Logging.Log(Tags.BUILDABLE, this);

            Assert.IsNotNull(_parent, "Must call StaticInitialise() before Initialise(...)");
            Helper.AssertIsNotNull(factoryProvider);


            _factoryProvider = factoryProvider;
            _targetFactories = _factoryProvider.Targets;
            _movementControllerFactory = _factoryProvider.MovementControllerFactory;
            _uiManager = uiManager;
            _buildTimeInDroneSeconds = numOfDronesRequired * buildTimeInS;
            HealthGainPerDroneS = maxHealth / _buildTimeInDroneSeconds;
            BuildProgressBoostable = _factoryProvider.BoostFactory.CreateBoostable();

            _clickHandler.SingleClick += ClickHandler_SingleClick;
            _clickHandler.DoubleClick += ClickHandler_DoubleClick;

            _healthBar.Initialise(this, followDamagable: true);

            Logging.Log(Tags.BUILDABLE, $"{this}:  _parent.SetActive(false);");
            //  _parent.SetActive(false);
        }

        public virtual void Activate(IPvPCruiser parentCruiser, IPvPCruiser enemyCruiser, IPvPCruiserSpecificFactories cruiserSpecificFactories)
        {
            _parent.SetActive(true);
            if (_parent.GetComponent<PvPBuildingWrapper>() is not null && IsServer)
            {
                _parent.GetComponent<PvPBuildingWrapper>().IsVisible = true;
            }
            ParentCruiser = parentCruiser;
            EnemyCruiser = enemyCruiser;
            _cruiserSpecificFactories = cruiserSpecificFactories;

            Faction = ParentCruiser.Faction;
            CallRpc_SyncFaction(Faction);
            _aircraftProvider = _cruiserSpecificFactories.AircraftProvider;
            _localBoosterBoostableGroup = _factoryProvider.BoostFactory.CreateBoostableGroup();
            _buildRateBoostableGroup = CreateBuildRateBoostableGroup(_factoryProvider.BoostFactory, _cruiserSpecificFactories.GlobalBoostProviders, BuildProgressBoostable);
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
            ParentCruiser = activationArgs.ParentCruiser;
            _droneConsumerProvider = ParentCruiser.DroneConsumerProvider;
            Faction = ParentCruiser.Faction;
            CallRpc_SyncFaction(Faction);
            EnemyCruiser = activationArgs.EnemyCruiser;

            _cruiserSpecificFactories = activationArgs.CruiserSpecificFactories;
            _aircraftProvider = activationArgs.CruiserSpecificFactories.AircraftProvider;

            BuildableState = PvPBuildableState.NotStarted;
            _cumulativeBuildProgressInDroneS = 0;

            _localBoosterBoostableGroup = _factoryProvider.BoostFactory.CreateBoostableGroup();
            _buildRateBoostableGroup = CreateBuildRateBoostableGroup(_factoryProvider.BoostFactory, _cruiserSpecificFactories.GlobalBoostProviders, BuildProgressBoostable);
        }

        public void Activate(TPvPActivationArgs activationArgs, PvPFaction faction)
        {
        }

        private IPvPBoostableGroup CreateBuildRateBoostableGroup(IPvPBoostFactory boostFactory, IPvPGlobalBoostProviders globalBoostProviders, IPvPBoostable buildProgressBoostable)
        {
            IPvPBoostableGroup buildRateBoostableGroup = boostFactory.CreateBoostableGroup();
            buildRateBoostableGroup.AddBoostable(buildProgressBoostable);

            IList<ObservableCollection<IPvPBoostProvider>> buildRateBoostProvidersList = new List<ObservableCollection<IPvPBoostProvider>>();
            AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);

            foreach (ObservableCollection<IPvPBoostProvider> buildRateBoostProviders in buildRateBoostProvidersList)
            {
                buildRateBoostableGroup.AddBoostProvidersList(buildRateBoostProviders);
            }

            return buildRateBoostableGroup;
        }

        /// <summary>
        /// To allow multiple boost provider sources.  Eg, for the ShieldGenerator:
        /// + Tacticals => Boost from Trident
        /// + Shields   => Boost from Raptor
        /// </summary>
        protected virtual void AddBuildRateBoostProviders(
            IPvPGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IPvPBoostProvider>> buildRateBoostProvidersList)
        {
            Logging.Log(Tags.BOOST, this);
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

        private void DroneConsumer_DroneNumChanged(object sender, PvPDroneNumChangedEventArgs e)
        {
            DroneNumChanged?.Invoke(this, e);
        }

        private void DroneConsumer_DroneStateChanged(object sender, PvPDroneStateChangedEventArgs e)
        {
            if (BuildableState != PvPBuildableState.Completed)
            {
                if (e.OldState == PvPDroneConsumerState.Idle)
                {
                    BuildableState = PvPBuildableState.InProgress;
                }
                else if (e.NewState == PvPDroneConsumerState.Idle)
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

            if (DroneConsumer.State != PvPDroneConsumerState.Idle)
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
                    Assert.IsTrue(DroneConsumer.State != PvPDroneConsumerState.Idle);

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
            }

        }

        protected virtual void OnUpdate() { }

        protected virtual void OnBuildableCompleted()
        {
            Logging.Log(Tags.BUILDABLE, this);

            CleanUpDroneConsumer();

            EnableRenderers(true);
            BuildableState = PvPBuildableState.Completed;

            _smokeInitialiser.Initialise(this, ShowSmokeWhenDestroyed);

            if (ConstructionCompletedSoundKey != null)
            {
                // _cruiserSpecificFactories.BuildableEffectsSoundPlayer.PlaySound(ConstructionCompletedSoundKey);
                PlayBuildableConstructionCompletedSound();
            }
            CompletedBuildable?.Invoke(this, EventArgs.Empty);
            CallRpc_ProgressControllerVisible(false);
            RepairCommand.EmitCanExecuteChanged();
        }

        protected virtual void OnBuildableCompleted_PvPClient()
        {
            BuildableState = PvPBuildableState.Completed;
            _smokeInitialiser.Initialise(this, ShowSmokeWhenDestroyed);
            CompletedBuildable?.Invoke(this, EventArgs.Empty);
            CallRpc_ProgressControllerVisible(false);
            RepairCommand.EmitCanExecuteChanged();
            ToggleDroneConsumerFocusCommand.EmitCanExecuteChanged();
        }

        protected virtual void PlayBuildableConstructionCompletedSound()
        {
            if (ConstructionCompletedSoundKey != null)
            {
                if (IsOwner)
                    _factoryProvider.Sound.PrioritisedSoundPlayer.PlaySound(ConstructionCompletedSoundKey);
            }
        }

        private void EnableRenderers(bool enabled)
        {
            if (IsServer)
                OnValueChangedIsEnableRenderes(enabled);

            Logging.Log(Tags.BUILDING, $"Renderer count: {InGameRenderers.Count}  enabled: {enabled}");

            foreach (Renderer renderer in InGameRenderers)
            {
                renderer.enabled = enabled;
            }
        }

        protected override void InternalDestroy()
        {
            Deactivate();
        }

        protected virtual void Deactivate()
        {
            Logging.Log(Tags.BUILDABLE, $"{this}:  _parent.SetActive(false);");

            Assert.IsTrue(_parent.activeSelf);
            _parent.SetActive(false);
            if (_parent.GetComponent<PvPBuildingWrapper>() is not null && IsServer)
            {
                _parent.GetComponent<PvPBuildingWrapper>().IsVisible = false;
            }
            Deactivated?.Invoke(this, EventArgs.Empty);
            Invoke("iDestroyParentGameObject", 1f);
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

            CallRpc_PlayDeathSound();
            //    _factoryProvider.Sound.SoundPlayer.PlaySound(_deathSound, transform.position);

            if (Faction == PvPFaction.Reds)
            {


                //    BattleSceneGod.AddDeadBuildable(PvPTargetType, (int)(buildTimeInS * numOfDronesRequired));

                //BattleSceneGod.ShowDeadBuildableStats();
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
            if (IsClient)
                CallRpc_ToggleDroneConsumerFocusCommandExecute();
            if (IsServer)
                ParentCruiser.DroneFocuser.ToggleDroneConsumerFocus(DroneConsumer, isTriggeredByPlayer: true);
        }


        protected virtual void CallRpc_ToggleDroneConsumerFocusCommandExecute()
        {
            if (IsServer)
                ToggleDroneConsumerFocusCommandExecute();
        }

        protected virtual void CallRpc_PlayDeathSound()
        {
            _factoryProvider.Sound.SoundPlayer.PlaySound(_deathSound, transform.position);
        }

        protected virtual void CallRpc_SyncFaction(PvPFaction faction)
        {
            
        }
    }
}
