using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.UI;
using BattleCruisers.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildings.Factories
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
				Assert.AreEqual(BuildableState.Completed, BuildableState);

				if (_unit != null)
				{
					Assert.IsNotNull(DroneConsumer);
					_droneManager.RemoveDroneConsumer(DroneConsumer);
					DroneConsumer = null;

					// FELIX  Delete in progress unit!
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
			Debug.Log("StartBuildingUnit()");

			_unitUnderConstruction = _buildableFactory.CreateUnit(_unit, this);

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
			base.OnDestroyed();

			// Destroy unit currently under production
			if (_unitUnderConstruction != null)
			{
				Destroy(_unitUnderConstruction);
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
