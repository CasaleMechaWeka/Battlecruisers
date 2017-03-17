using BattleCruisers.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildings
{
	public class Factory : Building
	{
		// FELIX  Use
		public int BuildPoints { set; private get; }
		public int NumOfAidingDrones { set; private get; }
		public Direction SpawnDirection { set; private get; }

		// public so that it will be copied via Instantiate
		public IList<Unit> units;

		// FELIX  Let the factory build more than one unit :P
		private AttackBoatController _unit;
		public AttackBoatController Unit 
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

		public override void Initialise(BattleCruisers.UI.UIManager uiManagerArg, BattleCruisers.Cruisers.Cruiser parentCruiser, BattleCruisers.Cruisers.Cruiser enemyCruiser, BuildingFactory buildingFactory)
		{
			base.Initialise(uiManagerArg, parentCruiser, enemyCruiser, buildingFactory);


		}

		void Start()
		{
			Debug.Log("Factory.Start()");

			BuildPoints = -1;
			NumOfAidingDrones = 0;
		}

		private void StartProducing()
		{
			if (BuildPoints < 0)
			{
				throw new InvalidOperationException();
			}

			// FELIX  Figure out how to speed this up with drones
			float productionTimeInS = _unit.BuildTimeInS / BuildPoints;
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

			unitAsRigidbody.velocity = new Vector2(directionMultiplier * _unit.VelocityInMPerS, 0);
		}
	}
}