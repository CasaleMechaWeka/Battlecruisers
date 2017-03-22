using BattleCruisers.Cruisers;
using BattleCruisers.UI;
using BattleCruisers.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildings.Factories
{
	public abstract class Factory : Building
	{
		protected BuildableFactory _buildableFactory;

		public UnitCategory unitCategory;
		public int buildPoints;

		// FELIX  Use
		public int NumOfAidingDrones { set; private get; }
		public Direction SpawnDirection { set; private get; }

		private Unit _unit;
		public Unit Unit 
		{ 
			set
			{
				StopProducing();
				_unit = value;
				if (_unit != null)
				{
					StartProducing();
				}
			}
			private get { return _unit; }
		}

		public override void Initialise(UIManager uiManager, Cruiser parentCruiser, Cruiser enemyCruiser, BuildableFactory buildableFactory)
		{
			base.Initialise(uiManager, parentCruiser, enemyCruiser, buildableFactory);
			_buildableFactory = buildableFactory;
		}

		public override void Initialise(BuildableObject buildable)
		{
			base.Initialise(buildable);
			Factory factory = buildable as Factory;
			Assert.IsNotNull(factory);
			_buildableFactory = factory._buildableFactory;
		}

		void Start()
		{
			Debug.Log("Factory.Start()");

			buildPoints = -1;
			NumOfAidingDrones = 0;
		}

		protected override void OnClicked()
		{
			base.OnClicked();
			_uiManager.ShowFactoryUnits(this);
		}

		private void StartProducing()
		{
			// FELIX  Figure out how to speed this up with drones
			float productionTimeInS = _unit.buildTimeInS;
			InvokeRepeating("ProduceUnit", productionTimeInS, productionTimeInS);
		}

		private void StopProducing()
		{
			CancelInvoke("ProduceUnit");
		}

		private void ProduceUnit()
		{
			Debug.Log("ProduceUnit()");

			// FELIX If cannot spawn unit, spawn AS SOON AS there is space.
			// So don't have to wait full build cycle!
			if (CanSpawnUnit(_unit))
			{
				Unit unit = _buildableFactory.CreateUnit(_unit);

				Vector3 spawnPosition = FindUnitSpawnPosition(unit);
				unit.transform.position = spawnPosition;
				unit.transform.rotation = transform.rotation;

				unit.faction = _parentCruiser.faction;
				unit.facingDirection = _parentCruiser.direction;

				OnUnitProduced(unit);
			}
		}
		
		// Check if there is space for the unit to be spawned, or
		// perhaps if unit maximum has been reached.
		protected virtual bool CanSpawnUnit(Unit unit)
		{
			return false;
		}

		protected abstract Vector3 FindUnitSpawnPosition(Unit unit);

		protected virtual void OnUnitProduced(Unit unit) { }
	}
}
