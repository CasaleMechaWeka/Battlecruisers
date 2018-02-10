using System;
using System.Collections.Generic;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Movement;
using BattleCruisers.Targets;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Timers;
using BattleCruisers.Utils.UIWrappers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Projectiles.Stats.Wrappers;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;

namespace BattleCruisers.Buildables
{
    public abstract class Buildable : Target, IBuildable, IPointerClickHandler
    {
        private float _cumulativeBuildProgressInDroneS;
        private float _buildTimeInDroneSeconds;
        private NumOfDronesTextController _numOfDronesText;
        private HealthBarController _healthBar;

        protected IUIManager _uiManager;
        protected ICruiser _parentCruiser;
        protected ICruiser _enemyCruiser;
        protected IDroneManager _droneManager;
        protected IDroneConsumerProvider _droneConsumerProvider;
        protected ITargetsFactory _targetsFactory;
        protected IMovementControllerFactory _movementControllerFactory;
        protected IAircraftProvider _aircraftProvider;
        protected IFactoryProvider _factoryProvider;
        protected IBoostableGroup _boostableGroup;
        protected BuildableProgressController _buildableProgress;

        public string buildableName;
        public string description;
        public int numOfDronesRequired;
        public float buildTimeInS;
        public SlotType slotType;

        private const float MAX_BUILD_PROGRESS = 1;
        private const float INITIAL_HEALTH = 1;
        // TEMP  Build cheat multiplier
        //private const float BUILD_CHEAT_MULTIPLIER = 10;
        //private const float BUILD_CHEAT_MULTIPLIER = 50;
        public const float BUILD_CHEAT_MULTIPLIER = 2;

        #region Properties
        public BuildableState BuildableState { get; private set; }
        public float BuildProgress { get; private set; }
        public int NumOfDronesRequired { get { return numOfDronesRequired; } }
        public float BuildTimeInS { get { return buildTimeInS; } }
        public SlotType SlotType { get { return slotType; } }
        protected abstract HealthBarController HealthBarController { get; }
        public IBoostable BuildProgressBoostable { get; private set; }
        public override Vector2 Size { get { return _buildableProgress.FillableImageSprite.bounds.size; } }
        public float CostInDroneS { get { return NumOfDronesRequired * BuildTimeInS; } }
        protected virtual ISoundKey DeathSoundKey { get { return SoundKeys.Explosions.Default; } }

        private IList<IDamageCapability> _damageCapabilities;
        public ReadOnlyCollection<IDamageCapability> DamageCapabilities { get; private set; }

        Quaternion IBuildable.Rotation 
        {
            get { return transform.rotation; }
            set { transform.rotation = value; } 
        }

        Vector2 IBuildable.Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

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

        private IList<Renderer> _inGameRenderers;
        private IList<Renderer> InGameRenderers
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

        protected virtual bool IsDroneConsumerFocusable { get { return DroneConsumer != null; } }

        public ICommand ToggleDroneConsumerFocusCommand { get; private set; }

        private CountdownController _deleteCountdown;
        private CountdownController DeleteCountdown
        {
            get
            {
                if (_deleteCountdown == null)
                {
                    _deleteCountdown = _factoryProvider.PrefabFactory.CreateDeleteCountdown(transform);
                }
                return _deleteCountdown;
            }
        }

        #region IComparableItem
        Sprite IComparableItem.Sprite { get { return _buildableProgress.FillableImageSprite; } }
        string IComparableItem.Description { get { return description; } }
        string IComparableItem.Name { get { return buildableName; } }
        #endregion IComparableItem
        #endregion Properties

        public event EventHandler StartedConstruction;
        public event EventHandler CompletedBuildable;
        public event EventHandler<BuildProgressEventArgs> BuildableProgress;
        public event EventHandler<DroneNumChangedEventArgs> DroneNumChanged;

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

            _damageCapabilities = new List<IDamageCapability>();
            this.DamageCapabilities = new ReadOnlyCollection<IDamageCapability>(_damageCapabilities);
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
                    // FELIX  Once attack capabilities are a set, can avoid this check :)
                    if (!_attackCapabilities.Contains(attackCapability))
                    {
                        _attackCapabilities.Add(attackCapability);
                    }
                }
            }
        }

        // Reuse text mesh for showing num of drones while building is being built.
        protected override ITextMesh GetRepairDroneNumText()
        {
            return _numOfDronesText.NumOfDronesText;
        }

        protected virtual IList<Renderer> GetInGameRenderers()
        {
            Renderer mainRenderer = GetComponent<Renderer>();
            Assert.IsNotNull(mainRenderer);
            return new List<Renderer>() { mainRenderer };
        }

        protected void Initialise(ICruiser parentCruiser, ICruiser enemyCruiser, IUIManager uiManager, IFactoryProvider factoryProvider)
        {
            Assert.IsNotNull(_numOfDronesText, "Must call StaticInitialise() before Initialise(...)");

            _parentCruiser = parentCruiser;
            _enemyCruiser = enemyCruiser;
            _droneManager = _parentCruiser.DroneManager;
            _droneConsumerProvider = _parentCruiser.DroneConsumerProvider;
            _uiManager = uiManager;
            _aircraftProvider = factoryProvider.AircraftProvider;

            _factoryProvider = factoryProvider;
            _targetsFactory = _factoryProvider.TargetsFactory;
            _movementControllerFactory = _factoryProvider.MovementControllerFactory;

            Faction = _parentCruiser.Faction;
            BuildableState = BuildableState.NotStarted;
            _buildTimeInDroneSeconds = numOfDronesRequired * buildTimeInS;
            _cumulativeBuildProgressInDroneS = 0;

            HealthGainPerDroneS = _buildTimeInDroneSeconds / maxHealth;

            _boostableGroup = _factoryProvider.BoostFactory.CreateBoostableGroup();
            BuildProgressBoostable = _factoryProvider.BoostFactory.CreateBoostable();
        }

        protected virtual void OnInitialised() { }

        private void DroneConsumer_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            if (DroneNumChanged != null)
            {
                DroneNumChanged.Invoke(this, e);
            }
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
            Health = INITIAL_HEALTH;

            SetupDroneConsumer(numOfDronesRequired);

            EnableRenderers(false);

            if (DroneConsumer.State != DroneConsumerState.Idle)
            {
                BuildableState = BuildableState.InProgress;
            }

            if (StartedConstruction != null)
            {
                StartedConstruction.Invoke(this, EventArgs.Empty);
            }
        }

        void Update()
        {
            if (BuildableState == BuildableState.InProgress)
            {
                Assert.IsTrue(DroneConsumer.State != DroneConsumerState.Idle);

                // Find build progress
                float buildProgressInDroneS = DroneConsumer.NumOfDrones * BuildProgressBoostable.BoostMultiplier * Time.deltaTime * BUILD_CHEAT_MULTIPLIER;
                _cumulativeBuildProgressInDroneS += buildProgressInDroneS;

				BuildProgress = _cumulativeBuildProgressInDroneS / _buildTimeInDroneSeconds;

                if (BuildProgress > MAX_BUILD_PROGRESS)
				{
					BuildProgress = MAX_BUILD_PROGRESS;
				}

                // Increase health with build progress
                float buildProgressIncrement = buildProgressInDroneS / _buildTimeInDroneSeconds;
                Health += buildProgressIncrement * MaxHealth;

                if (BuildableProgress != null)
                {
                    BuildableProgress.Invoke(this, new BuildProgressEventArgs(this));
                }

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

            if (CompletedBuildable != null)
            {
                CompletedBuildable.Invoke(this, EventArgs.Empty);
            }

            RepairCommand.EmitCanExecuteChanged();
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

            _boostableGroup.CleanUp();

            _factoryProvider.SoundManager.PlaySound(DeathSoundKey, transform.position);
        }

        protected void SetupDroneConsumer(int numOfDrones)
        {
            Assert.IsNull(DroneConsumer);
            DroneConsumer = _droneConsumerProvider.RequestDroneConsumer(numOfDrones, isHighPriority: true);
            _droneConsumerProvider.ActivateDroneConsumer(DroneConsumer);
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


        private void ToggleDroneConsumerFocusCommandExecute()
        {
            _droneManager.ToggleDroneConsumerFocus(DroneConsumer);
        }

        public void InitiateDelete()
        {
            DeleteCountdown.Begin(Destroy);
        }

        public void CancelDelete()
        {
            DeleteCountdown.Cancel();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (DeleteCountdown.IsInProgress)
            {
                CancelDelete();
            }
            else
            {
				OnClicked();
            }
        }

        protected virtual void OnClicked() { }
    }
}
