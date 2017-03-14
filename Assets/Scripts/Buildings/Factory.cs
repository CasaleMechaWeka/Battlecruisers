using BattleCruisers.Units;
using System;
using UnityEngine;

namespace BattleCruisers.Buildings
{
	public interface IFactory
	{
		// FELIX  Let this be generic
		// FactoryUnit FactoryUnit { ; }
		AttackBoatController Unit { set; }
		int BuildPoints { set; }
		int NumOfAidingDrones { set; }
		Direction SpawnDirection { set; }
	}

	public class Factory : MonoBehaviour, IFactory
	{
		// FELIX  Use
		public int BuildPoints { set; private get; }
		public int NumOfAidingDrones { set; private get; }
		public Direction SpawnDirection { set; private get; }

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