using BattleCruisers.Buildables.ActivationArgs;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects.Smoke;
using BattleCruisers.Movement;
using BattleCruisers.Targets.Factories;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils.Timers;
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
        private NumOfDronesTextController _numOfDronesText;
        private HealthBarController _healthBar;
        private IClickHandler _clickHandler;
        // Keep reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private SmokeInitialiser _smokeInitialiser;
#pragma warning restore CS0414  // Variable is assigned but never used

        protected IUIManager _uiManager;
        protected ICruiser _enemyCruiser;
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

        public string buildableName;
        public string description;
        public int numOfDronesRequired;
        public float buildTimeInS;

        private const float MAX_BUILD_PROGRESS = 1;

        #region Properties
        public BuildableState BuildableState { get; private set; }
        public float BuildProgress { get; private set; }
        public int NumOfDronesRequired => numOfDronesRequired;
        public float BuildTimeInS => buildTimeInS;
        protected abstract HealthBarController HealthBarController { get; }
        public IBoostable BuildProgressBoostable { get; private set; }
        public override Vector2 Size => _buildableProgress.FillableImage.sprite.bounds.size;
        public float CostInDroneS => NumOfDronesRequired * BuildTimeInS;
        protected virtual ISoundKey DeathSoundKey => SoundKeys.Explosions.Default;
        protected abstract PrioritisedSoundKey ConstructionCompletedSoundKey { get; }
        public ICruiser ParentCruiser { get; private set; }
        protected virtual bool ShowSmokeWhenDestroyed => false;

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

        protected virtual bool IsDroneConsumerFocusable => DroneConsumer != null;

        public ICommand ToggleDroneConsumerFocusCommand { get; private set; }

        private CountdownController _deleteCountdown;
        private CountdownController DeleteCountdown
        {
            get
            {
                if (_deleteCountdown == null)
                {
                    _deleteCountdown = _factoryProvider.PrefabFactory.CreateDeleteCountdown(transform);

                    // Position below buildable
                    float yOffset = -Size.y / 2;
                    _deleteCountdown.transform.position = transform.position + Transform.Up * yOffset;
                }
                return _deleteCountdown;
            }
        }

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
        Sprite IComparableItem.Sprite => _buildableProgress.FillableImage.sprite;
        string IComparableItem.Description => description;
        string IComparableItem.Name => buildableName;
        #endregion IComparableItem
        #endregion Properties

        public event EventHandler StartedConstruction;
        public event EventHandler CompletedBuildable;
        public event EventHandler<BuildProgressEventArgs> BuildableProgress;
        public event EventHandler<DroneNumChangedEventArgs> DroneNumChanged;
        public event EventHandler Clicked;
        // FELIX  Invoke!!!
        public event EventHandler Deactivated;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            _buildableProgress = gameObject.GetComponentInChildren<BuildableProgressController>(includeInactive: true);
            Assert.IsNotNull(_buildableProgress);
            _buildableProgress.Initialise();

            _healthBar = HealthBarController;
            Assert.IsNotNull(_healthBar);
            _healthBar.Initialise(this, followDamagable: true);

            ToggleDroneConsumerFocusCommand = new Command(ToggleDroneConsumerFocusCommandExecute, () => IsDroneConsumerFocusable);

            _numOfDronesText = gameObject.GetComponentInChildren<NumOfDronesTextController>(includeInactive: true);
            Assert.IsNotNull(_numOfDronesText);
            _numOfDronesText.Initialise(this);

            ClickHandlerWrapper clickHandlerWrapper = GetComponent<ClickHandlerWrapper>();
            Assert.IsNotNull(clickHandlerWrapper);
            _clickHandler = clickHandlerWrapper.GetClickHandler();

            _damageCapabilities = new List<IDamageCapability>();
            this.DamageCapabilities = new ReadOnlyCollection<IDamageCapability>(_damageCapabilities);

            _smokeInitialiser = GetComponentInChildren<SmokeInitialiser>(includeInactive: true);
            Assert.IsNotNull(_smokeInitialiser);
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

        // Reuse text mesh for showing num of drones while building is being built.
        protected override ITextMesh GetRepairDroneNumText()
        {
            return _numOfDronesText.NumOfDronesText;
        }

        protected override List<SpriteRenderer> GetInGameRenderers()
        {
            SpriteRenderer mainRenderer = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(mainRenderer);
            return new List<SpriteRenderer>() { mainRenderer };
        }

        // FELIX  Make sure this is only called once :)  By prefab factory!!!
        public virtual void Initialise(IUIManager uiManager, IFactoryProvider factoryProvider)
        {
            Assert.IsNotNull(_numOfDronesText, "Must call StaticInitialise() before Initialise(...)");
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
        }

        public virtual void Activate(TActivationArgs activationArgs)
        {
            Assert.IsNotNull(activationArgs);

            ParentCruiser = activationArgs.ParentCruiser;
            _droneConsumerProvider = ParentCruiser.DroneConsumerProvider;
            Faction = ParentCruiser.Faction;

            _enemyCruiser = activationArgs.EnemyCruiser;

            _cruiserSpecificFactories = activationArgs.CruiserSpecificFactories;
            _aircraftProvider = activationArgs.CruiserSpecificFactories.AircraftProvider;

            BuildableState = BuildableState.NotStarted;
            _cumulativeBuildProgressInDroneS = 0;

            _localBoosterBoostableGroup = _factoryProvider.BoostFactory.CreateBoostableGroup();
            _buildRateBoostableGroup = CreateBuildRateBoostableGroup(_factoryProvider.BoostFactory, _cruiserSpecificFactories.GlobalBoostProviders, BuildProgressBoostable);
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
            Logging.Log(Tags.BOOST, this.ToString());
        }

        /// <summary>
        /// Called only once, when an object is first instantiated.
        /// </summary>
        protected virtual void OnInitialised() { }

        /// <summary>
        /// Called every time an object is taken from an object pool and activated.  Can happen
        /// multiple times for buildables as they are recycled.
        /// </summary>
        protected virtual void OnActivated() { }

        private void ClickHandler_SingleClick(object sender, EventArgs e)
        {
            Logging.Log(Tags.BUILDABLE, this.ToString());

            if (DeleteCountdown.IsInProgress)
            {
                CancelDelete();
            }
            else
            {
                OnSingleClick();
            }

            Clicked?.Invoke(this, EventArgs.Empty);
        }

        protected abstract void OnSingleClick();

        private void ClickHandler_DoubleClick(object sender, EventArgs e)
        {
            Logging.Log(Tags.BUILDABLE, this.ToString());
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

        public void StartConstruction()
        {
            _healthTracker.SetMinHealth();

            SetupDroneConsumer(numOfDronesRequired);

            EnableRenderers(false);

            if (DroneConsumer.State != DroneConsumerState.Idle)
            {
                BuildableState = BuildableState.InProgress;
            }

            StartedConstruction?.Invoke(this, EventArgs.Empty);
        }

        void Update()
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
            CleanUpDroneConsumer();

            EnableRenderers(true);
            BuildableState = BuildableState.Completed;

            CompletedBuildable?.Invoke(this, EventArgs.Empty);

            RepairCommand.EmitCanExecuteChanged();

            _cruiserSpecificFactories.BuildableEffectsSoundPlayer.PlaySound(ConstructionCompletedSoundKey);
            _smokeInitialiser.Initialise(this, ShowSmokeWhenDestroyed);
        }

        private void EnableRenderers(bool enabled)
        {
            foreach (Renderer renderer in InGameRenderers)
            {
                renderer.enabled = enabled;
            }
        }

        // All buildables are wrapped by a UnitWrapper or BuildingWrapper, which contains
        // both the target and the health bar.  Hence destroy wrapper, so health bar
        // gets destroyed at the same time as the target.
        protected override void InternalDestroy()
        {
            Assert.IsNotNull(transform.parent);
            Destroy(transform.parent.gameObject);
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

            _factoryProvider.Sound.SoundPlayer.PlaySound(DeathSoundKey, transform.position);
        }

        protected void SetupDroneConsumer(int numOfDrones)
        {
            Assert.IsNull(DroneConsumer);
            DroneConsumer = _droneConsumerProvider.RequestDroneConsumer(numOfDrones);
            _droneConsumerProvider.ActivateDroneConsumer(DroneConsumer);

            Logging.Log(Tags.BUILDABLE, $"{buildableName}  numOfDrones: {numOfDrones}  DroneConsumer.NumOfDrones: {DroneConsumer.NumOfDrones}");
        }

        protected void CleanUpDroneConsumer()
        {
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

        public void InitiateDelete()
        {
            if (BuildableState == BuildableState.NotStarted)
            {
                Destroy();
            }
            else
            {
                DeleteCountdown.Begin(Destroy);
            }
        }

        public void CancelDelete()
        {
            DeleteCountdown.Cancel();
        }
    }
}
