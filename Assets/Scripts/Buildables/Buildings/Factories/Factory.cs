using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Pools;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public abstract class Factory : Building, IFactory, IDroneConsumerProvider
	{
        private IUnitSpawnPositionFinder _unitSpawnPositionFinder;
        private IUnitSpawnDecider _unitSpawnDecider;
        private IPool<Unit, BuildableActivationArgs> _unitPool;

        public abstract UnitCategory UnitCategory { get; }

		public event EventHandler<UnitStartedEventArgs> UnitStarted;
        public event EventHandler<UnitCompletedEventArgs> UnitCompleted;
        public event EventHandler NewUnitChosen;
        public event EventHandler UnitUnderConstructionDestroyed;

        #region Properties
        public abstract LayerMask UnitLayerMask { get; }
        public IUnit UnitUnderConstruction { get; private set; }
        public override bool IsBoostable => true;

        private ObservableValue<bool> _isUnitPaused;
        public IObservableValue<bool> IsUnitPaused
        {
            get { return _isUnitPaused; }
        }

        private IBuildableWrapper<IUnit> _unitWrapper;
		public IBuildableWrapper<IUnit> UnitWrapper 
		{ 
			private set	
			{
				Logging.Log(Tags.FACTORY, $"{_unitWrapper} > {value}");
				Assert.AreEqual(BuildableState.Completed, BuildableState);

                if (!ReferenceEquals(_unitWrapper, value))
                {
	                if (_unitWrapper != null)
	                {
                        CleanUpDroneConsumer();
	                    DestroyUnitUnderConstruction();
                        _isUnitPaused.Value = false;
                        _unitPool = null;
	                }

	                _unitWrapper = value;

	                if (_unitWrapper != null)
                    {
                        SetupDroneConsumer(_unitWrapper.Buildable.NumOfDronesRequired, showDroneFeedback: false);
                        EnsureDroneConsumerHasHighestPriority();
                        _unitPool = _factoryProvider.PoolProviders.UnitToPoolMap.GetPool(_unitWrapper.Buildable);

                        NewUnitChosen?.Invoke(this, EventArgs.Empty);
                    }
                }
			}
			get { return _unitWrapper; }
		}

        public int NumOfDrones
        {
            get
            {
                return DroneConsumer != null ? DroneConsumer.NumOfDrones : 0;
            }
        }

        public AudioClip selectedSound;
        public IAudioClipWrapper SelectedSound { get; private set; }

        public AudioClip unitSelectedSound;
        public IAudioClipWrapper UnitSelectedSound { get; private set; }
        #endregion Properties

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            _isUnitPaused = new ObservableValue<bool>(false);

            Assert.IsNotNull(selectedSound);
            SelectedSound = new AudioClipWrapper(selectedSound);

            Assert.IsNotNull(unitSelectedSound);
            UnitSelectedSound = new AudioClipWrapper(unitSelectedSound);
        }

        public override void Activate(BuildingActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            _unitSpawnPositionFinder = CreateSpawnPositionFinder();
            _unitSpawnDecider = _factoryProvider.SpawnDeciderFactory.CreateSpawnDecider(this, _unitSpawnPositionFinder);
        }

        protected abstract IUnitSpawnPositionFinder CreateSpawnPositionFinder();

        protected override void OnSingleClick()
		{
			base.OnSingleClick();

			if (Faction == Faction.Blues)
			{
				_uiManager.ShowFactoryUnits(this);
			}
		}

        // PERF  Don't need to do this every update :)
		protected override void OnUpdate()
		{
            Logging.Verbose(Tags.FACTORY, $"UnitWrapper: {UnitWrapper}  _isUnitPaused.Value: {_isUnitPaused.Value}  UnitUnderConstruction: {UnitUnderConstruction}");
            if (UnitWrapper != null)
            {
                Logging.Verbose(Tags.FACTORY, $"Can spawn: {_unitSpawnDecider.CanSpawnUnit(UnitWrapper.Buildable)}");
            }

            if (UnitWrapper != null 
                && !_isUnitPaused.Value
				&& (UnitUnderConstruction == null || UnitUnderConstruction.BuildableState == BuildableState.Completed)
				&& _unitSpawnDecider.CanSpawnUnit(UnitWrapper.Buildable))
			{
				StartBuildingUnit();
			}
		}

		private void StartBuildingUnit()
		{
            Logging.LogMethod(Tags.FACTORY);
            if (EnemyCruiser == null || ParentCruiser == null)
            {
                return;
            }
            BuildableActivationArgs activationArgs = new BuildableActivationArgs(ParentCruiser, EnemyCruiser, _cruiserSpecificFactories);
            UnitUnderConstruction = _unitPool.GetItem(activationArgs);

            UnitUnderConstruction.DroneConsumerProvider = this;

			Vector3 spawnPosition = _unitSpawnPositionFinder.FindSpawnPosition(UnitUnderConstruction);
            UnitUnderConstruction.Position = spawnPosition;
            UnitUnderConstruction.Rotation = transform.rotation;

			UnitUnderConstruction.StartedConstruction += Unit_BuildingStarted;
			UnitUnderConstruction.CompletedBuildable += Unit_CompletedBuildable;
            UnitUnderConstruction.Destroyed += UnitUnderConstruction_Destroyed;

            UnitUnderConstruction.AddBuildRateBoostProviders(_parentSlot.BoostProviders);
            UnitUnderConstruction.StartConstruction();

            if (UnitUnderConstruction.ParentCruiser.IsPlayerCruiser) {
                string logName = UnitUnderConstruction.PrefabName.ToUpper().Replace("(CLONE)","");
#if LOG_ANALYTICS
    Debug.Log("Analytics: " + logName);
#endif
                IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
                try
                {
                    AnalyticsService.Instance.CustomData("Battle_Buildable_Unit", applicationModel.DataProvider.GameModel.Analytics(applicationModel.Mode.ToString(), logName, applicationModel.UserWonSkirmish));
                    AnalyticsService.Instance.Flush();
                }
                catch (ConsentCheckException ex)
                { 
                    Debug.Log(ex.Message);
                }
               
            }

		}

		protected virtual void Unit_BuildingStarted(object sender, EventArgs e) 
		{
            Logging.Log(Tags.FACTORY, sender.ToString());

            IUnit unit = sender.Parse<IUnit>();
            UnitStarted?.Invoke(this, new UnitStartedEventArgs(unit));
		}

		private void Unit_CompletedBuildable(object sender, EventArgs e)
		{
            Logging.Log(Tags.FACTORY, sender.ToString());
			
            UnitCompleted?.Invoke(this, new UnitCompletedEventArgs(UnitUnderConstruction));
            CleanUpUnitUnderConstruction();
		}

        private void UnitUnderConstruction_Destroyed(object sender, DestroyedEventArgs e)
        {
            Logging.Log(Tags.FACTORY, sender.ToString());

            CleanUpUnitUnderConstruction();
            UnitUnderConstructionDestroyed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// The unit under construction will request a drone consumer.  The factory
        /// keeps hold of a drone consumer for the current unit, so simply pass
        /// on the factory's drone consumer.
        /// </summary>
        public IDroneConsumer RequestDroneConsumer(int numOfDronesRequired)
		{
            Logging.LogMethod(Tags.FACTORY);

			Assert.IsNotNull(DroneConsumer);
            Assert.AreEqual(DroneConsumer.NumOfDronesRequired, numOfDronesRequired, "DroneConsumer.NumOfDronesRequired: " + DroneConsumer.NumOfDronesRequired + " != " + numOfDronesRequired);
			return DroneConsumer;
		}

        public void ActivateDroneConsumer(IDroneConsumer droneConsumer) { }

        public void ReleaseDroneConsumer(IDroneConsumer droneConsumer) { }

        protected override void OnDestroyed()
		{
            Logging.LogMethod(Tags.FACTORY);

			base.OnDestroyed();
			DestroyUnitUnderConstruction();
		}

		private void DestroyUnitUnderConstruction()
		{
            Logging.LogMethod(Tags.FACTORY);

			if (UnitUnderConstruction != null
                && !UnitUnderConstruction.IsDestroyed
                && UnitUnderConstruction.BuildableState != BuildableState.Completed)
			{
				UnitUnderConstruction.Destroy();
			}
		}

		private void CleanUpUnitUnderConstruction()
		{
            Logging.LogMethod(Tags.FACTORY);

            UnitUnderConstruction.StartedConstruction -= Unit_BuildingStarted;
            UnitUnderConstruction.CompletedBuildable -= Unit_CompletedBuildable;
            UnitUnderConstruction.Destroyed -= UnitUnderConstruction_Destroyed;
			UnitUnderConstruction = null;
		}

        public void StartBuildingUnit(IBuildableWrapper<IUnit> unit)
        {
            Logging.Log(Tags.FACTORY, unit?.ToString());
            UnitWrapper = unit;
        }

        public void StopBuildingUnit()
        {
            Logging.LogMethod(Tags.FACTORY);
            UnitWrapper = null;
        }

        public void PauseBuildingUnit()
        {
            Logging.LogMethod(Tags.FACTORY);

            if (UnitWrapper != null
                && !_isUnitPaused.Value)
            {
                _droneConsumerProvider.ReleaseDroneConsumer(DroneConsumer);
                _isUnitPaused.Value = true;
            }
        }

        public void ResumeBuildingUnit()
        {
            Logging.LogMethod(Tags.FACTORY);

            if (_isUnitPaused.Value)
            {
                Assert.IsNotNull(UnitWrapper);

                _droneConsumerProvider.ActivateDroneConsumer(DroneConsumer);
                EnsureDroneConsumerHasHighestPriority();
                _isUnitPaused.Value = false;
            }
        }

        private void EnsureDroneConsumerHasHighestPriority()
        {
            Logging.LogMethod(Tags.FACTORY);

            if (DroneConsumer.State == DroneConsumerState.Idle)
            {
                ParentCruiser.DroneFocuser.ToggleDroneConsumerFocus(DroneConsumer, isTriggeredByPlayer: false);
            }
        }

        protected override void ToggleDroneConsumerFocusCommandExecute()
        {
            Logging.Log(Tags.FACTORY, $"_isUnitPaused.Value: {_isUnitPaused.Value}");

            if (_isUnitPaused.Value)
            {
                // Cannot focus on drone consumer if they are not activated.
                // Hence, activate drone consumer before focusing on them :)
                _droneConsumerProvider.ActivateDroneConsumer(DroneConsumer);
                _isUnitPaused.Value = false;
            }

            base.ToggleDroneConsumerFocusCommandExecute();
        }
    }
}
