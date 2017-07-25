using System;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public abstract class Factory : Building, IDroneConsumerProvider
	{
		private IUnit _lastUnitProduced;
		protected IUnit _unitUnderConstruction;

		public UnitCategory unitCategory;

		private const float SPAWN_RADIUS_MULTIPLIER = 1.2f;

		public event EventHandler<StartedConstructionEventArgs> StartedBuildingUnit;

		#region Properties
		private IBuildableWrapper<IUnit> _unitWrapper;
		public IBuildableWrapper<IUnit> UnitWrapper 
		{ 
			set	
			{
				Logging.Log(Tags.FACTORY, "set_UnitWrapper: " + _unitWrapper + " > " + value);
				Assert.AreEqual(BuildableState.Completed, BuildableState);

				if (_unitWrapper != null)
				{
					Assert.IsNotNull(DroneConsumer);
					_droneConsumerProvider.ReleaseDroneConsumer(DroneConsumer);
					DroneConsumer = null;
					DestroyUnitUnderConstruction();
				}

				_unitWrapper = value;

				if (_unitWrapper != null)
				{
					DroneConsumer = _droneConsumerProvider.RequestDroneConsumer(_unitWrapper.Buildable.NumOfDronesRequired);
                    Assert.IsNull(DroneConsumer);
					_droneConsumerProvider.ActivateDroneConsumer(DroneConsumer);
				}
			}
			private get { return _unitWrapper; }
		}

		protected abstract LayerMask UnitLayerMask { get; }
		#endregion Properties

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

			_unitUnderConstruction = _prefabFactory.CreateUnit(_unitWrapper);
			_unitUnderConstruction.Initialise(_parentCruiser, _enemyCruiser, _uiManager, _factoryProvider);
			_unitUnderConstruction.DroneConsumerProvider = this;

			Vector3 spawnPosition = FindUnitSpawnPosition(_unitUnderConstruction);
            _unitUnderConstruction.Position = spawnPosition;
            _unitUnderConstruction.Rotation = transform.rotation;

			_unitUnderConstruction.StartedConstruction += Unit_StartedConstruction;
			_unitUnderConstruction.CompletedBuildable += Unit_CompletedBuildable;

			_unitUnderConstruction.StartConstruction();

			if (StartedBuildingUnit != null)
			{
				StartedBuildingUnit.Invoke(this, new StartedConstructionEventArgs(_unitUnderConstruction));
			}
		}

		protected abstract Vector3 FindUnitSpawnPosition(IUnit unit);

		protected virtual void Unit_StartedConstruction(object sender, EventArgs e) 
		{ 
			_unitUnderConstruction.StartedConstruction -= Unit_StartedConstruction;

			IUnit unit = sender as IUnit;
			Assert.IsNotNull(unit);
			_lastUnitProduced = unit;
		}

		private void Unit_CompletedBuildable(object sender, EventArgs e)
		{
			CleanUpUnitUnderConstruction();
		}
		
		public IDroneConsumer RequestDroneConsumer(int numOfDronesRequired)
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
			if (_unitUnderConstruction != null)
			{
				_unitUnderConstruction.Destroy();
				CleanUpUnitUnderConstruction();
			}
		}

		private void CleanUpUnitUnderConstruction()
		{
			_unitUnderConstruction.CompletedBuildable -= Unit_CompletedBuildable;
			_unitUnderConstruction = null;
		}

		public void ActivateDroneConsumer(IDroneConsumer droneConsumer) { }

		public void ReleaseDroneConsumer(IDroneConsumer droneConsumer) { }
	}
}
