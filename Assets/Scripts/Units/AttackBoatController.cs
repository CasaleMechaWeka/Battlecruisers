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
using BattleCruisers.Buildings.Turrets;
using BattleCruisers.Units.Detectors;


namespace BattleCruisers.Units
{
	/// <summary>
	/// Assumptions:
	/// 1. Boats only move horizontally, and are all at the same height
	/// 2. Boats only engage one enemy at a time
	/// 3. All enemies will come towards the front of the boat, and all allies will come
	/// 	towards the rear of the boat.
	/// 4. Boat will only stop to fight enemies.  Either this boat is destroyed, or the
	/// 	enemy, in which case this boat will continue moving.
	/// </summary>
	public class AttackBoatController : Unit
	//public class AttackBoatController : MonoBehaviour, IDetectorControllerListener
	{
		private Rigidbody2D _rigidBody;
		private float _fireDelayInS = 0.25f; // The amount of time before firing starts.
		private FactionObject _enemyUnit;
		private FactionObject _blockingFriendlyUnit;
		private int _directionMultiplier;

		public EnemyDetector enemyDetector;
		public FriendDetector friendDetector;
		// FELIX  Allow to vary depending on boat?
		public Rigidbody2D shellPrefab;

		public ITurretStats TurretStats { private get; set; }

		void Start() 
		{
			_rigidBody = GetComponent<Rigidbody2D>();
			_directionMultiplier = facingDirection == Direction.Right ? 1 : -1;

			// FELIX  Inject, don't hardcode
			TurretStats = new TurretStats(0.5f, 1f, 10f, 3f, ignoreGravity: true);

			// FELIX  Don't hardcode string, add to Constants class?
			// FELIX  Set from unity via public fields
//			EnemyUnitDetector enemyDetector = transform.Find("EnemyDetector").GetComponent<EnemyUnitDetector>();
			enemyDetector.gameObject.SetActive(true);
			enemyDetector.OnEntered = OnEnemyEntered;
			enemyDetector.OwnFaction = faction;
//
//			FriendlyUnitDetector friendDetector = transform.Find("FriendDetector").GetComponent<FriendlyUnitDetector>();
			friendDetector.gameObject.SetActive(true);
			friendDetector.OnEntered = OnFriendEntered;
			friendDetector.OnExited = OnFriendExited;
			friendDetector.OwnFaction = faction;
		}

		void Update()
		{
			if (_rigidBody.velocity.x == 0)
			{
				// Check if enemy has been destroyed
				if (_enemyUnit != null && _enemyUnit.IsDestroyed)
				{
					StopAttacking();
					_enemyUnit = null;
				}

				// Check if blocking friendly has been destroyed
				if (_blockingFriendlyUnit != null && _blockingFriendlyUnit.IsDestroyed)
				{
					_blockingFriendlyUnit = null;
				}

				if (_enemyUnit == null && _blockingFriendlyUnit == null)
				{
					StartMoving();
				}
			}
		}

		private void StopAttacking()
		{
			CancelInvoke("Attack");
		}

		/// <summary>
		/// Stop and shoot.
		/// </summary>
		private void OnEnemyEntered(FactionObject enemey)
		{
			_enemyUnit = enemey;
			StopMoving();
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

			shell.GetComponent<IShellController>().Damage = TurretStats.Damage;
			shell.gravityScale = 0;

			float velocityX = TurretStats.BulletVelocityInMPerS * directionMultiplier;
			shell.velocity = new Vector2(velocityX, 0);
		}

		private void OnFriendEntered(FactionObject friend)
		{
			if (IsObjectInFront(friend))
			{
				_blockingFriendlyUnit = friend;
				StopMoving();
			}
		}

		private void OnFriendExited(FactionObject friend)
		{
			if (IsObjectInFront(friend))
			{
				StartMoving();
			}
		}

		private bool IsObjectInFront(FactionObject factionObject)
		{
			return (facingDirection == Direction.Right
					&& factionObject.gameObject.transform.position.x > transform.position.x)
				|| (facingDirection == Direction.Left
					&& factionObject.gameObject.transform.position.x < transform.position.x);
		}

		private void StartMoving()
		{
			_rigidBody.velocity = new Vector2(velocityInMPerS * _directionMultiplier, 0);
		}

		private void StopMoving()
		{
			_rigidBody.velocity = new Vector2(0, 0);
		}

		public void TakeDamage(float damage)
		{
			Debug.Log("AttackBoatController.TakeDamage()");

			health -= damage;

			if (health <= 0)
			{
				Destroy(gameObject);
			}
		}
	}
}
