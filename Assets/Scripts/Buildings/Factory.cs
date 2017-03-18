using BattleCruisers.Cruisers;
using BattleCruisers.UI;
using BattleCruisers.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildings
{
	public class Factory : Building
	{
		public UnitCategory unitCategory;
		public int buildPoints;

		// FELIX  Use
		public int NumOfAidingDrones { set; private get; }
		public Direction SpawnDirection { set; private get; }

		// FELIX  Let the factory build more than one unit :P
		private Unit _unit;
		public Unit Unit 
		{ 
			set
			{
				_unit = value;
				if (_unit == null)
				{
					StopProducing();
				}
				else
				{
					StartProducing();
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
			// FELIX  TEMP
			return;

			if (buildPoints < 0)
			{
				throw new InvalidOperationException();
			}

			// FELIX  Figure out how to speed this up with drones
			float productionTimeInS = _unit.buildTimeInS / buildPoints;
			InvokeRepeating("ProduceUnit", productionTimeInS, productionTimeInS);
		}

		private void StopProducing()
		{
			CancelInvoke("ProduceUnit");
		}

		private void ProduceUnit()
		{
			Debug.Log("ProduceUnit()");

			// FELIX  Determine direction

			float yRotationInDegrees = 0;
			float directionMultiplier = 1;

			if (SpawnDirection == Direction.Left)
			{
				yRotationInDegrees = 180f;
				directionMultiplier = -1;
			}

			Vector3 spawnPosition = transform.position;
			spawnPosition.x += directionMultiplier * 3;

			AttackBoatController unit = Instantiate(_unit, spawnPosition, Quaternion.Euler(new Vector3(0, yRotationInDegrees, 0))) as AttackBoatController;

			// FELIX
	//		unit.tag = "Enemy";
			
			Rigidbody2D unitAsRigidbody = unit.GetComponent<Rigidbody2D>();

			unitAsRigidbody.velocity = new Vector2(directionMultiplier * _unit.velocityInMPerS, 0);
		}
	}
}