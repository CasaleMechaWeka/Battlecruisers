using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public abstract class Factory : Building, IFactory, IDroneConsumerProvider
	{
        private IUnitSpawnPositionFinder _unitSpawnPositionFinder;
        private IUnitSpawnDecider _unitSpawnDecider;

        public abstract UnitCategory UnitCategory { get; }

		private const float SPAWN_RADIUS_MULTIPLIER = 1.2f;

		public event EventHandler<UnitStartedEventArgs> UnitStarted;
        public event EventHandler<UnitCompletedEventArgs> UnitCompleted;
        public event EventHandler NewUnitChosen;

        #region Properties
        protected override ISoundKey DeathSoundKey => SoundKeys.Deaths.Building3;
        public abstract LayerMask UnitLayerMask { get; }
        public IUnit UnitUnderConstruction { get; private set; }

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
	                }

	                _unitWrapper = value;

	                if (_unitWrapper != null)
                    {
                        SetupDroneConsumer(_unitWrapper.Buildable.NumOfDronesRequired);
                        EnsureDroneConsumerHasHighestPriority();

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
        #endregion Properties

        protected override void OnStaticInitialised()
        {
            base.OnStaticInitialised();
            _isUnitPaused = new ObservableValue<bool>(false);
        }

        protected override void OnInitialised()
        {
            base.OnInitialised();

            _unitSpawnPositionFinder = CreateSpawnPositionFinder();
            _unitSpawnDecider = _factoryProvider.SpawnDeciderFactory.CreateSpawnDecider(this, _unitSpawnPositionFinder);
        }

        protected abstract IUnitSpawnPositionFinder CreateSpawnPositionFinder();

        /// <summary>
        /// Buildings only become repairable after they are completed.  So buildings
        /// reuse the text mesh for the number of building drones for the number
        /// of repairing drones.  
        /// 
        /// However, factories use the text mesh even AFTER they finish building,
        /// to show the number of drones used in constructing units.  Hence, we
        /// need a different text mesh to show the number of repair drones
        /// (ie, both text meshes can be visible at the same time).
        /// </summary>
        protected override ITextMesh GetRepairDroneNumText()
        {
            TextMesh repairDroneNumText = transform.FindNamedComponent<TextMesh>("RepairDroneNumText");
            return new TextMeshWrapper(repairDroneNumText);
        }

        protected override void OnSingleClick()
		{
			base.OnSingleClick();

			if (Faction == Faction.Blues)
			{
				_uiManager.ShowFactoryUnits(this);
			}
		}

        // FELIX  PERF  Don't need to do this every update :)
		protected override void OnUpdate()
		{
			if (_unitWrapper != null 
				&& (UnitUnderConstruction == null || UnitUnderConstruction.BuildableState == BuildableState.Completed)
				&& _unitSpawnDecider.CanSpawnUnit(_unitWrapper.Buildable))
			{
				StartBuildingUnit();
			}
		}
		
		private void StartBuildingUnit()
		{
            Logging.LogMethod(Tags.FACTORY);

			UnitUnderConstruction = _factoryProvider.PrefabFactory.CreateUnit(_unitWrapper);
			UnitUnderConstruction.Initialise(ParentCruiser, _enemyCruiser, _uiManager, _factoryProvider, _cruiserSpecificFactories);
			UnitUnderConstruction.DroneConsumerProvider = this;

			Vector3 spawnPosition = _unitSpawnPositionFinder.FindSpawnPosition(UnitUnderConstruction);
            UnitUnderConstruction.Position = spawnPosition;
            UnitUnderConstruction.Rotation = transform.rotation;

			UnitUnderConstruction.StartedConstruction += Unit_BuildingStarted;
			UnitUnderConstruction.CompletedBuildable += Unit_CompletedBuildable;
            UnitUnderConstruction.Destroyed += UnitUnderConstruction_Destroyed;

            UnitUnderConstruction.AddBuildRateBoostProviders(_parentSlot.BoostProviders);

			UnitUnderConstruction.StartConstruction();
		}

		protected virtual void Unit_BuildingStarted(object sender, EventArgs e) 
		{ 
			UnitUnderConstruction.StartedConstruction -= Unit_BuildingStarted;

            IUnit unit = sender.Parse<IUnit>();

            UnitStarted?.Invoke(this, new UnitStartedEventArgs(unit));
		}

		private void Unit_CompletedBuildable(object sender, EventArgs e)
		{
			UnitCompleted?.Invoke(this, new UnitCompletedEventArgs(UnitUnderConstruction));

            CleanUpUnitUnderConstruction();
		}

        private void UnitUnderConstruction_Destroyed(object sender, DestroyedEventArgs e)
        {
            CleanUpUnitUnderConstruction();
        }

        /// <summary>
        /// The unit under construction will request a drone consumer.  The factory
        /// keeps hold of a drone consumer for the current unit, so simply pass
        /// on the factory's drone consumer.
        /// </summary>
        public IDroneConsumer RequestDroneConsumer(int numOfDronesRequired)
		{
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
			if (UnitUnderConstruction != null
                && !UnitUnderConstruction.IsDestroyed
                && UnitUnderConstruction.BuildableState != BuildableState.Completed)
			{
				UnitUnderConstruction.Destroy();
			}
		}

		private void CleanUpUnitUnderConstruction()
		{
			UnitUnderConstruction.CompletedBuildable -= Unit_CompletedBuildable;
            UnitUnderConstruction.Destroyed -= UnitUnderConstruction_Destroyed;
			UnitUnderConstruction = null;
		}

        public void StartBuildingUnit(IBuildableWrapper<IUnit> unit)
        {
            UnitWrapper = unit;
        }

        public void StopBuildingUnit()
        {
            UnitWrapper = null;
        }

        public void PauseBuildingUnit()
        {
            if (_unitWrapper != null
                && !_isUnitPaused.Value)
            {
                _droneConsumerProvider.ReleaseDroneConsumer(DroneConsumer);
                _isUnitPaused.Value = true;
            }
        }

        public void ResumeBuildingUnit()
        {
            if (_isUnitPaused.Value)
            {
                Assert.IsNotNull(_unitWrapper);

                _droneConsumerProvider.ActivateDroneConsumer(DroneConsumer);
                EnsureDroneConsumerHasHighestPriority();
                _isUnitPaused.Value = false;
            }
        }

        private void EnsureDroneConsumerHasHighestPriority()
        {
            if (DroneConsumer.State == DroneConsumerState.Idle)
            {
                ParentCruiser.DroneFocuser.ToggleDroneConsumerFocus(DroneConsumer, isTriggeredByPlayer: false);
            }
        }

        protected override void ToggleDroneConsumerFocusCommandExecute()
        {
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
