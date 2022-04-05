using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Pools;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Effects.Smoke;
using BattleCruisers.Movement;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.Targets.Factories;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables
{
    public abstract class Buildable<TActivationArgs> : Target, IBuildable, IPoolable<TActivationArgs>
        where TActivationArgs : BuildableActivationArgs
    {
        private float _cumulativeBuildProgressInDroneS;
        private float _buildTimeInDroneSeconds;
        private IClickHandler _clickHandler;
        // Keep reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private SmokeInitialiser _smokeInitialiser;
#pragma warning restore CS0414  // Variable is assigned but never used
        // All buildables are wrapped by a UnitWrapper or BuildingWrapper, which contains
        // both the target and the health bar.
        private GameObject _parent;
        private IDroneFeedback _droneFeedback;

        protected IUIManager _uiManager;
        protected IDroneConsumerProvider _droneConsumerProvider;
        protected ITargetFactoriesProvider _targetFactories;
        protected IMovementControllerFactory _movementControllerFactory;
        protected IAircraftProvider _aircraftProvider;
        protected IFactoryProvider _factoryProvider;
        protected ICruiserSpecificFactories _cruiserSpecificFactories;
        // Boost resulting from global cruiser bonuses
        protected IBoostableGroup _buildRateBoostableGroup;
        // Boost resulting from adjacent local boosters
        protected IBoostableGroup _localBoosterBoostableGroup;
        protected BuildableProgressController _buildableProgress;

        public string stringKeyName;
        public string keyName{get;set;}
        public int numOfDronesRequired;
        public float buildTimeInS;

        private IAudioClipWrapper _deathSound;
        [Header("Sounds")]
        public AudioClip deathSound;

        private const float MAX_BUILD_PROGRESS = 1;

        #region Properties
        public BuildableState BuildableState { get; private set; }
        public float BuildProgress { get; private set; }
        public int NumOfDronesRequired => numOfDronesRequired;
        public float BuildTimeInS => buildTimeInS;
        public IBoostable BuildProgressBoostable { get; private set; }
        public override Vector2 Size => _buildableProgress.FillableImage.sprite.bounds.size;
        public float CostInDroneS => NumOfDronesRequired * BuildTimeInS;
        protected virtual PrioritisedSoundKey ConstructionCompletedSoundKey => null;
        public ICruiser ParentCruiser { get; private set; }
        public ICruiser EnemyCruiser { get; private set; }
        protected virtual bool ShowSmokeWhenDestroyed => false;
        public string PrefabName => _parent.name;

        private HealthBarController _healthBar;
        public IHealthBar HealthBar => _healthBar;

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
        public ICommand ToggleDroneConsumerFocusCommand { get; private set; }
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
        public event EventHandler<BuildProgressEventArgs> BuildableProgress;
        public event EventHandler<DroneNumChangedEventArgs> DroneNumChanged;
        public event EventHandler Clicked;
        public event EventHandler Deactivated;

        public virtual void StaticInitialise(GameObject parent, HealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(commonStrings);
            keyName = stringKeyName;
            Helper.AssertIsNotNull(parent, healthBar);

            _parent = parent;
            _healthBar = healthBar;

            _buildableProgress = gameObject.GetComponentInChildren<BuildableProgressController>(includeInactive: true);
            Assert.IsNotNull(_buildableProgress);
            _buildableProgress.Initialise();

            ToggleDroneConsumerFocusCommand = new Command(ToggleDroneConsumerFocusCommandExecute, () => IsDroneConsumerFocusable);

            ClickHandlerWrapper clickHandlerWrapper = GetComponent<ClickHandlerWrapper>();
            Assert.IsNotNull(clickHandlerWrapper);
            _clickHandler = clickHandlerWrapper.GetClickHandler();

            _damageCapabilities = new List<IDamageCapability>();
            this.DamageCapabilities = new ReadOnlyCollection<IDamageCapability>(_damageCapabilities);

            _smokeInitialiser = GetComponentInChildren<SmokeInitialiser>(includeInactive: true);
            Assert.IsNotNull(_smokeInitialiser);

            Assert.IsNotNull(deathSound);
            _deathSound = new AudioClipWrapper(deathSound);
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
            {
                foreach (TargetType attackCapability in damageStat.AttackCapabilities)
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
                return new List<SpriteRenderer>(){};
            }
            return new List<SpriteRenderer>() { mainRenderer };
        }

        /// <summary>
        /// Called only once, when an object is first instantiated.
        /// </summary>
        public virtual void Initialise(IUIManager uiManager, IFactoryProvider factoryProvider)
        {
            Logging.Log(Tags.BUILDABLE, this);

            Assert.IsNotNull(_parent, "Must call StaticInitialise() before Initialise(...)");
            Helper.AssertIsNotNull(uiManager, factoryProvider);

            _uiManager = uiManager;
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
        }

        public virtual void Activate(ICruiser parentCruiser, ICruiser enemyCruiser, ICruiserSpecificFactories cruiserSpecificFactories)
        {
            _parent.SetActive(true);
            ParentCruiser = parentCruiser;
            EnemyCruiser = enemyCruiser;
            _cruiserSpecificFactories = cruiserSpecificFactories;

            Faction = ParentCruiser.Faction;
            _aircraftProvider = _cruiserSpecificFactories.AircraftProvider;
            _localBoosterBoostableGroup = _factoryProvider.BoostFactory.CreateBoostableGroup();
            _buildRateBoostableGroup = CreateBuildRateBoostableGroup(_factoryProvider.BoostFactory, _cruiserSpecificFactories.GlobalBoostProviders, BuildProgressBoostable);
        }

        /// <summary>
        /// Called every time an object is taken from an object pool and activated.  Can happen
        /// multiple times for buildables as they are recycled.
        /// </summary>
        public virtual void Activate(TActivationArgs activationArgs)
        {
            Logging.Log(Tags.BUILDABLE, $"{this}:  _parent.SetActive(true);");
            Assert.IsNotNull(activationArgs);

            Assert.IsFalse(_parent.activeSelf);
            _parent.SetActive(true);

            ParentCruiser = activationArgs.ParentCruiser;
            _droneConsumerProvider = ParentCruiser.DroneConsumerProvider;
            Faction = ParentCruiser.Faction;

            EnemyCruiser = activationArgs.EnemyCruiser;

            _cruiserSpecificFactories = activationArgs.CruiserSpecificFactories;
            _aircraftProvider = activationArgs.CruiserSpecificFactories.AircraftProvider;

            BuildableState = BuildableState.NotStarted;
            _cumulativeBuildProgressInDroneS = 0;

            _localBoosterBoostableGroup = _factoryProvider.BoostFactory.CreateBoostableGroup();
            _buildRateBoostableGroup = CreateBuildRateBoostableGroup(_factoryProvider.BoostFactory, _cruiserSpecificFactories.GlobalBoostProviders, BuildProgressBoostable);
        }

        public void Activate(TActivationArgs activationArgs, Faction faction)
        {
        }

        private IBoostableGroup CreateBuildRateBoostableGroup(IBoostFactory boostFactory, IGlobalBoostProviders globalBoostProviders, IBoostable buildProgressBoostable)
        {
            IBoostableGroup buildRateBoostableGroup = boostFactory.CreateBoostableGroup();
            buildRateBoostableGroup.AddBoostable(buildProgressBoostable);

            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList = new List<ObservableCollection<IBoostProvider>>();
            AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);

            foreach (ObservableCollection<IBoostProvider> buildRateBoostProviders in buildRateBoostProvidersList)
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
            IGlobalBoostProviders globalBoostProviders, 
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
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

        private void DroneConsumer_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            DroneNumChanged?.Invoke(this, e);
        }

        private void DroneConsumer_DroneStateChanged(object sender, DroneStateChangedEventArgs e)
        {
            if (BuildableState != BuildableState.Completed)
            {
                if (e.OldState == DroneConsumerState.Idle)
                {
                    BuildableState = BuildableState.InProgress;
                }
                else if (e.NewState == DroneConsumerState.Idle)
                {
                    BuildableState = BuildableState.Paused;
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
                BuildableState = BuildableState.InProgress;
            }

            StartedConstruction?.Invoke(this, EventArgs.Empty);
        }

        // PERF  Doesn't need to be every update :)
        public void Update()
        {
            if (BuildableState == BuildableState.InProgress)
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

                BuildableProgress?.Invoke(this, new BuildProgressEventArgs(this));

                if (_cumulativeBuildProgressInDroneS >= _buildTimeInDroneSeconds)
                {
                    OnBuildableCompleted();
                }
            }

            OnUpdate();
        }

        protected virtual void OnUpdate() { }

        protected virtual void OnBuildableCompleted()
        {
            Logging.Log(Tags.BUILDABLE, this);

            CleanUpDroneConsumer();

            EnableRenderers(true);
            BuildableState = BuildableState.Completed;

            _smokeInitialiser.Initialise(this, ShowSmokeWhenDestroyed);

            if (ConstructionCompletedSoundKey != null)
            {
                _cruiserSpecificFactories.BuildableEffectsSoundPlayer.PlaySound(ConstructionCompletedSoundKey);
            }

            CompletedBuildable?.Invoke(this, EventArgs.Empty);
            RepairCommand.EmitCanExecuteChanged();
        }

        private void EnableRenderers(bool enabled)
        {
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
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            if (DroneConsumer != null)
            {
                CleanUpDroneConsumer();
            }

            _localBoosterBoostableGroup.CleanUp();
            _buildRateBoostableGroup.CleanUp();

            _factoryProvider.Sound.SoundPlayer.PlaySound(_deathSound, transform.position);

            if (Faction == Faction.Reds)
            {
                BattleSceneGod.AddDeadBuildable(TargetType, (int)(buildTimeInS*numOfDronesRequired));
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
                && BuildableState == BuildableState.Completed;
        }

        protected virtual void ToggleDroneConsumerFocusCommandExecute()
        {
            ParentCruiser.DroneFocuser.ToggleDroneConsumerFocus(DroneConsumer, isTriggeredByPlayer: true);
        }
    }
}
