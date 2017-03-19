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
//					StartProducing();
					// FELIX  TEMP  Just produce one unit for now
					ProduceUnit();
				}
			}
			private get { return _unit; }
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

			Unit unit = Instantiate<Unit>(_unit);

			Vector3 spawnPosition = FindUnitSpawnPosition(unit);
			unit.transform.position = spawnPosition;
			unit.transform.rotation = transform.rotation;

			unit.faction = _parentCruiser.faction;
			unit.facingDirection = _parentCruiser.direction;
		}

		protected abstract Vector3 FindUnitSpawnPosition(Unit unit);
	}
}
