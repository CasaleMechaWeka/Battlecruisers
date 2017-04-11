using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.UI;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Factories
{
	public class BuildProgressEventArgs : EventArgs
	{
		public float BuildProgress { get; private set; }

		public BuildProgressEventArgs(float buildProgress)
		{
			BuildProgress = buildProgress;
		}
	}

	// FELIX  Be able to be able to pause factory?
	public abstract class Factory : Building, IDroneConsumerProvider
	{
		protected Unit _unitUnderConstruction;

		public UnitCategory unitCategory;
		public Direction SpawnDirection { set; private get; }

		private Unit _unit;
		public Unit Unit 
		{ 
			set	
			{
				Logging.Log(Tags.FACTORY, $"set_Unit: {_unit} > {value}");

				Assert.AreEqual(BuildableState.Completed, BuildableState);

				if (_unit != null)
				{
					Assert.IsNotNull(DroneConsumer);
					_droneManager.RemoveDroneConsumer(DroneConsumer);
					DroneConsumer = null;

					DestroyUnitUnderConstruction();
				}

				_unit = value;

				if (_unit != null)
				{
					DroneConsumer = new DroneConsumer(_unit.numOfDronesRequired);
					_droneManager.AddDroneConsumer(DroneConsumer);
				}
			}
			private get { return _unit; }
		}

		protected override void OnClicked()
		{
			base.OnClicked();

			// FELIX  Do not show units if AI cruiser factory
			_uiManager.ShowFactoryUnits(this);
		}

		protected override void OnUpdate()
		{
			if (_unit != null 
				&& (_unitUnderConstruction == null || _unitUnderConstruction.BuildableState == BuildableState.Completed)
				&& CanSpawnUnit(_unit))
			{
				StartBuildingUnit();
			}
		}
		
		// Check if there is space for the unit to be spawned, or
		// perhaps if unit maximum has been reached.
		protected virtual bool CanSpawnUnit(Unit unit)
		{
			return false;
		}
		
		private void StartBuildingUnit()
		{
			Logging.Log(Tags.FACTORY, "StartBuildingUnit()");

			_unitUnderConstruction = _buildableFactory.CreateUnit(_unit);
			_unitUnderConstruction.DroneConsumerProvider = this;
			_unitUnderConstruction.Initialise(_uiManager, _parentCruiser, _enemyCruiser, _buildableFactory);

			Vector3 spawnPosition = FindUnitSpawnPosition(_unitUnderConstruction);
			_unitUnderConstruction.transform.position = spawnPosition;
			_unitUnderConstruction.transform.rotation = transform.rotation;

			_unitUnderConstruction.faction = _parentCruiser.faction;
			_unitUnderConstruction.facingDirection = _parentCruiser.direction;

			_unitUnderConstruction.StartedConstruction += Unit_StartedConstruction;
			_unitUnderConstruction.CompletedBuildable += Unit_CompletedBuildable;

			_unitUnderConstruction.StartConstruction();
		}

		protected abstract Vector3 FindUnitSpawnPosition(Unit unit);
		
		protected virtual void Unit_StartedConstruction(object sender, EventArgs e) 
		{ 
			_unitUnderConstruction.StartedConstruction -= Unit_StartedConstruction;
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
				Destroy(_unitUnderConstruction.gameObject);
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
