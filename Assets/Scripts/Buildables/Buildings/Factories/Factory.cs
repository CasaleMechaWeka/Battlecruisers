using System;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Utils;
using BattleCruisers.Utils.UIWrappers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public abstract class Factory : Building, IFactory, IDroneConsumerProvider
	{
		private IUnit _lastUnitProduced;
		protected IUnit _unitUnderConstruction;

        public abstract UnitCategory UnitCategory { get; }

		private const float SPAWN_RADIUS_MULTIPLIER = 1.2f;
        // FELIX  Use or delete :)
        private const float POST_UNIT_DESTRUCTION_DELAY_IN_S = 1.5f;

		public event EventHandler<StartedConstructionEventArgs> StartedBuildingUnit;
        public event EventHandler<CompletedConstructionEventArgs> CompletedBuildingUnit;

        #region Properties
        private IBuildableWrapper<IUnit> _unitWrapper;
		public IBuildableWrapper<IUnit> UnitWrapper 
		{ 
			set	
			{
				Logging.Log(Tags.FACTORY, "set_UnitWrapper: " + _unitWrapper + " > " + value);
				Assert.AreEqual(BuildableState.Completed, BuildableState);

                if (!ReferenceEquals(_unitWrapper, value))
                {
	                if (_unitWrapper != null)
	                {
                        CleanUpDroneConsumer();
	                    DestroyUnitUnderConstruction();
	                }

	                _unitWrapper = value;

	                if (_unitWrapper != null)
	                {
                        SetupDroneConsumer(_unitWrapper.Buildable.NumOfDronesRequired);
	                }
				}
			}
			get { return _unitWrapper; }
		}

		protected abstract LayerMask UnitLayerMask { get; }

        public int NumOfDrones
        {
            get
            {
                return DroneConsumer != null ? DroneConsumer.NumOfDrones : 0;
            }
        }
        #endregion Properties

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

        protected override void OnClicked()
		{
			base.OnClicked();

			if (Faction == Faction.Blues)
			{
				_uiManager.ShowFactoryUnits(this);
			}
		}

		protected override void OnUpdate()
		{
			if (_unitWrapper != null 
				&& (_unitUnderConstruction == null || _unitUnderConstruction.BuildableState == BuildableState.Completed)
				&& CanSpawnUnit(_unitWrapper.Buildable))
			{
				StartBuildingUnit();
			}
		}
		
		/// <returns><c>true</c> if the last produced unit is not blocking the spawn point, otherwise <c>false</c>.</returns>
		protected virtual bool CanSpawnUnit(IUnit unit)
		{
			if (_lastUnitProduced != null && !_lastUnitProduced.IsDestroyed)
			{
				Vector3 spawnPositionV3 = FindUnitSpawnPosition(unit);
				Vector2 spawnPositionV2 = new Vector2(spawnPositionV3.x, spawnPositionV3.y);
				float spawnRadius = SPAWN_RADIUS_MULTIPLIER * unit.Size.x;
				Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPositionV2, spawnRadius, UnitLayerMask);

				foreach (Collider2D collider in colliders)
				{
                    if (collider.gameObject == _lastUnitProduced.GameObject)
					{
						return false;
					}
				}
			}

			return true;
		}

		private void StartBuildingUnit()
		{
			Logging.Log(Tags.FACTORY, "StartBuildingUnit()");

			_unitUnderConstruction = _factoryProvider.PrefabFactory.CreateUnit(_unitWrapper);
			_unitUnderConstruction.Initialise(_parentCruiser, _enemyCruiser, _uiManager, _factoryProvider);
			_unitUnderConstruction.DroneConsumerProvider = this;

			Vector3 spawnPosition = FindUnitSpawnPosition(_unitUnderConstruction);
            _unitUnderConstruction.Position = spawnPosition;
            _unitUnderConstruction.Rotation = transform.rotation;

			_unitUnderConstruction.StartedConstruction += Unit_StartedConstruction;
			_unitUnderConstruction.CompletedBuildable += Unit_CompletedBuildable;
            _unitUnderConstruction.Destroyed += UnitUnderConstruction_Destroyed;

            _boostableGroup.AddBoostable(_unitUnderConstruction.BuildProgressBoostable);

			_unitUnderConstruction.StartConstruction();
		}

		protected abstract Vector3 FindUnitSpawnPosition(IUnit unit);

		protected virtual void Unit_StartedConstruction(object sender, EventArgs e) 
		{ 
			_unitUnderConstruction.StartedConstruction -= Unit_StartedConstruction;

            IUnit unit = sender.Parse<IUnit>();
			_lastUnitProduced = unit;

            if (StartedBuildingUnit != null)
            {
                StartedBuildingUnit.Invoke(this, new StartedConstructionEventArgs(unit));
            }
		}

		private void Unit_CompletedBuildable(object sender, EventArgs e)
		{
			if (CompletedBuildingUnit != null)
			{
				CompletedBuildingUnit.Invoke(this, new CompletedConstructionEventArgs(_unitUnderConstruction));
			}

            CleanUpUnitUnderConstruction();
		}

        private void UnitUnderConstruction_Destroyed(object sender, DestroyedEventArgs e)
        {
            CleanUpUnitUnderConstruction();
        }

        public IDroneConsumer RequestDroneConsumer(int numOfDronesRequired, bool isHighPriority)
		{
			Assert.IsNotNull(DroneConsumer);
			Assert.AreEqual(DroneConsumer.NumOfDronesRequired, numOfDronesRequired);
			return DroneConsumer;
		}

		protected override void OnDestroyed()
		{
			Logging.Log(Tags.FACTORY, "OnDestroyed()");

			base.OnDestroyed();
			DestroyUnitUnderConstruction();
		}

		private void DestroyUnitUnderConstruction()
		{
			if (_unitUnderConstruction != null
                && !_unitUnderConstruction.IsDestroyed
                && _unitUnderConstruction.BuildableState != BuildableState.Completed)
			{
				_unitUnderConstruction.Destroy();
				CleanUpUnitUnderConstruction();
			}
		}

		private void CleanUpUnitUnderConstruction()
		{
            _boostableGroup.RemoveBoostable(_unitUnderConstruction.BuildProgressBoostable);
			_unitUnderConstruction.CompletedBuildable -= Unit_CompletedBuildable;
            _unitUnderConstruction.Destroyed -= UnitUnderConstruction_Destroyed;
			_unitUnderConstruction = null;
		}

		public void ActivateDroneConsumer(IDroneConsumer droneConsumer) { }

		public void ReleaseDroneConsumer(IDroneConsumer droneConsumer) { }
	}
}
