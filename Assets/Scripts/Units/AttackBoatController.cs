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

/// <summary>
/// Assumptions:
/// 1. Boats only move horizontally, and are all at the same height
/// 2. Boats only engage one enemy at a time
/// 3. All enemies will come towards the front of the boat, and all allies will come
/// 	towards the rear of the boat.
/// 4. Boat will only stop to fight enemies.  Either this boat is destroyed, or the
/// 	enemy, in which case this boat will continue moving.
/// </summary>
public class AttackBoatController : Unit, IDamagable
//public class AttackBoatController : MonoBehaviour, IDetectorControllerListener
{
	private Rigidbody2D _rigidBody;
	private float _fireDelayInS = 0.25f; // The amount of time before firing starts.
	private IUnit _enemyUnit;
	private IUnit _blockingFriendlyUnit;

	// FELIX  Allow to vary depending on boat?
	public Rigidbody2D shellPrefab;

	// FELIX  TEMP
	public float startingVelocityX;
	public float startingHealth;
	public UnitType type;
	public Direction direction;

	public float VelocityInMPerS { get; set; }
	public ITurretStats TurretStats { private get; set; }
	public int BuildTimeInS { get; set;}

	void Start() 
	{
		_rigidBody = GetComponent<Rigidbody2D>();

		// FELIX  Inject, don't hardcode
		TurretStats = new TurretStats(0.5f, 1f, 10f, 3f, ignoreGravity: true);
		Health = startingHealth;
		Type = type;
		FacingDirection = direction;
		
		// FELIX TEMP
		VelocityInMPerS = startingVelocityX;
		_rigidBody.velocity = new Vector2(VelocityInMPerS, 0);

		// FELIX  Don't hardcode string, add to Constants class?
		EnemyUnitDetector enemyDetector = transform.Find("EnemyDetector").GetComponent<EnemyUnitDetector>();
		enemyDetector.OnEntered = OnEnemyEntered;
		enemyDetector.OwnType = Type;

		FriendlyUnitDetector friendDetector = transform.Find("FriendDetector").GetComponent<FriendlyUnitDetector>();
		friendDetector.OnEntered = OnFriendEntered;
		friendDetector.OnExited = OnFriendExited;
		friendDetector.OwnType = Type;
	}

	void Update()
	{
		if (_rigidBody.velocity.x == 0)
		{
			// Check if enemy has been destroyed
			if (_enemyUnit != null && _enemyUnit.IsDestroyed)
			{
				StopAttacking();
				_rigidBody.velocity = new Vector2(VelocityInMPerS, 0);
			}

			// Check if blocking friendly has been destroyed
		}
	}

	private void StopAttacking()
	{
		CancelInvoke("Attack");
	}

	/// <summary>
	/// Stop and shoot.
	/// </summary>
	private void OnEnemyEntered(IUnit enemeyUnit)
	{
		_enemyUnit = enemeyUnit;
		_rigidBody.velocity = new Vector2(0, 0);
		StartAttacking();
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

	private void OnFriendEntered(IUnit friendlyUnit)
	{
		if ((FacingDirection == Direction.Right
		    	&& friendlyUnit.GameObject.transform.position.x > transform.position.x)
			|| (FacingDirection == Direction.Left
		 	   && friendlyUnit.GameObject.transform.position.x < transform.position.x))
		{
			// Friendly unit is in front of us
			_rigidBody.velocity = new Vector2(0, 0);
		}
	}

	private void OnFriendExited(IUnit friendlyUnit)
	{
		_rigidBody.velocity = new Vector2(VelocityInMPerS, 0);
	}

	public void TakeDamage(float damage)
	{
		Debug.Log("AttackBoatController.TakeDamage()");

		Health -= damage;

		if (Health <= 0)
		{
			Destroy(gameObject);
		}
	}
}
