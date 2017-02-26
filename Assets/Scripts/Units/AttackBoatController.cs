using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FELIX  Create parent boat class
// FELIX  Create Unit class and interface
// FELIX  Behaviour
// 1. Friendly boat => stop
// 2. Friendly boat leaves/gets destroyed => Advance
// 3. Enemy => Stop & attack
// 4. Enemy leaves/gets destroyed => Stop attacking & advance
// 5. Collision, die?
public class AttackBoatController : MonoBehaviour
//public class AttackBoatController : MonoBehaviour, IDetectorControllerListener
{
	private Rigidbody2D _rigidBody;
	private float _fireDelayInS = 0.25f; // The amount of time before firing starts.

	// FELIX  Allow to vary depending on boat?
	public Rigidbody2D shellPrefab;

	// FELIX  TEMP
	public float startingVelocityX;

	public float VelocityInMPerS { set; get; }
	public ITurretStats TurretStats { set; private get; }
	public int BuildTimeInS { set; get; }

	void Start() 
	{
		_rigidBody = GetComponent<Rigidbody2D>();

		// FELIX  Don't hardcode string, add to Constants class?
		IDetectionController enemyDetector = transform.Find("EnemyDetector").GetComponent<IDetectionController>();
		enemyDetector.OnEntered = OnEnemyEntered;
		enemyDetector.OnExited = OnEnemyExited;

		// FELIX  Inject, don't hardcode
		TurretStats = new TurretStats(0.5f, 1f, 10, 3f, ignoreGravity: true);

		// FELIX TEMP
		_rigidBody.velocity = new Vector2(startingVelocityX, 0);
	}

	/// <summary>
	/// Stop and shoot.
	/// </summary>
	private void OnEnemyEntered(Collider2D collider)
	{
		// FELIX  TEMP
//		_rigidBody.velocity = -1 * _rigidBody.velocity;

		// Stop
		_rigidBody.velocity = new Vector2(0, 0);

		// Start shooting
//		StartAttacking();
		Attack();
	}

	// FELIX  Extract firing functionality, common with artillery
	private void StartAttacking()
	{
		if (TurretStats == null)
		{
			throw new InvalidOperationException();
		}

		float fireIntervalInS = 1 / TurretStats.FireRatePerS;
		InvokeRepeating("Attack", _fireDelayInS, fireIntervalInS);
	}

	/// <summary>
	/// Stop shooting and start moving.
	/// </summary>
	private void OnEnemyExited(Collider2D collider)
	{
		CancelInvoke("Attack");
	}

	private void Attack()
	{
		Debug.Log("AttackBoatController.Attack()");

		if (TurretStats == null)
		{
			throw new InvalidOperationException();
		}

		float directionMultiplier;
		float spawnOffsetX;

		if (transform.rotation.y == 0)
		{
			directionMultiplier = 1;
			spawnOffsetX = 1.5f;
		}
		else
		{
			directionMultiplier = -1;
			spawnOffsetX = -1.5f;
		}

		Vector3 spawnPosition = transform.position;
		spawnPosition.x += spawnOffsetX;

		Debug.Log($"spawnPosition: {spawnPosition.x}, {spawnPosition.y}");

		Rigidbody2D shell = Instantiate(shellPrefab, spawnPosition, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;

		shell.GetComponent<IBulletController>().Damage = TurretStats.Damage;
		shell.gravityScale = 0;

		float velocityX = TurretStats.BulletVelocityInMPerS * directionMultiplier;
		shell.velocity = new Vector2(velocityX, 0);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
//		Debug.Log("AttackBoatController.OnTriggerEnter2D()");
	}
}
