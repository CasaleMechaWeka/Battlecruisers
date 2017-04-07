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
		protected IDroneConsumer _droneConsumer;
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
					Assert.IsNotNull(_droneConsumer);
					_droneManager.RemoveDroneConsumer(_droneConsumer);
					_droneConsumer = null;

					// FELIX  Delete in progress unit!
				}

				_unit = value;

				if (_unit != null)
				{
					_droneConsumer = new DroneConsumer(_unit.numOfDronesRequired);
					_droneManager.AddDroneConsumer(_droneConsumer);
					// FELIX  Grey out units until factory has completed building
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

			_unitUnderConstruction.StartConstruction();
		}

		protected abstract Vector3 FindUnitSpawnPosition(Unit unit);
		
		protected virtual void Unit_StartedConstruction(object sender, EventArgs e) { }
		
		public IDroneConsumer RequestDroneConsumer(int numOfDronesRequired)
		{
			Assert.IsNotNull(_droneConsumer);
			Assert.AreEqual(_droneConsumer.NumOfDronesRequired, numOfDronesRequired);
			return _droneConsumer;
		}

		public void ActivateDroneConsumer(IDroneConsumer droneConsumer) { }

		public void ReleaseDroneConsumer(IDroneConsumer droneConsumer) { }
	}
}
