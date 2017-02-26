using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FELIX  Create parent boat class
// FELIX  Create Unit class and interface
public class AttackBoatController : MonoBehaviour
//public class AttackBoatController : MonoBehaviour, IDetectorControllerListener
{
	private Rigidbody2D _rigidBody;

	public float VelocityInMPerS { set; get; }
	public ITurretStats TurretStats { set; private get; }
	public int BuildTimeInS { set; get; }

	public CircleCollider2D friendlyUnitTrigger;
	// FELIX  Set trigger size depending on turret stats
	public CircleCollider2D enemyUnitTrigger;

	void Start() 
	{
		_rigidBody = GetComponent<Rigidbody2D>();

		// FELIX  Don't hardcode string, add to Constants class?
		IDetectionController enemyDetector = transform.Find("EnemyDetector").GetComponent<IDetectionController>();
		enemyDetector.OnEntered = OnEnemyEntered;
	}

	private void OnEnemyEntered(Collider2D collider)
	{
		// Stop
		_rigidBody.velocity = new Vector2(0, 0);

		// Start shooting
	}

	// FELIX  Behaviour
	// 1. Friendly boat => stop
	// 2. Friendly boat leaves/gets destroyed => Advance
	// 3. Enemy => Stop & attack
	// 4. Enemy leaves/gets destroyed => Stop attacking & advance
	void OnTriggerEnter2D(Collider2D collider)
	{
//		Debug.Log("AttackBoatController.OnTriggerEnter2D()");
	}
}
