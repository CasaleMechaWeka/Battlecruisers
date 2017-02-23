using System;
using UnityEngine;

public interface IFactory
{
	// FELIX  Let this be generic
	// FactoryUnit FactoryUnit { ; }
	AttackBoatController Unit { set; }
	int BuildPoints { set; }
	int NumOfAidingDrones { set; }
}

public class Factory : MonoBehaviour, IFactory
{
	// FELIX  Use
	public int BuildPoints { set; private get; }
	public int NumOfAidingDrones { set; private get; }

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

		Vector3 spawnPosition = transform.position;
		spawnPosition.x += 3;
		AttackBoatController unit = Instantiate(_unit, spawnPosition, Quaternion.Euler(new Vector3(0, 0, 0))) as AttackBoatController;
		Rigidbody2D unitAsRigidbody = unit.GetComponent<Rigidbody2D>();

		Debug.Log("Boat velocity: " + _unit.VelocityInMPerS);

		unitAsRigidbody.velocity = new Vector2(12, 0);
//		unitAsRigidbody.velocity = new Vector2(_unit.VelocityInMPerS, 0);
	}
}