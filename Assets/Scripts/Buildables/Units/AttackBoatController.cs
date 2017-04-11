using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units.Detectors;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.UI;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// FELIX  Create parent boat class
namespace BattleCruisers.Buildables.Units
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
		private ShellStats _shellStats;

		public EnemyDetector enemyDetector;
		public FriendDetector friendDetector;
		// FELIX  Allow to vary depending on boat?
		public Rigidbody2D shellPrefab;
		public ShellSpawnerController shellSpawner;
		public TurretStats turretStats;

		public override float Damage { get { return turretStats.DamagePerS; } }

		void Start() 
		{
			_rigidBody = GetComponent<Rigidbody2D>();
			_directionMultiplier = facingDirection == Direction.Right ? 1 : -1;

			_shellStats = new ShellStats(shellPrefab, turretStats.damage, turretStats.ignoreGravity, turretStats.bulletVelocityInMPerS);
			shellSpawner.Initialise(_shellStats);

			enemyDetector.OnEntered = OnEnemyEntered;
			enemyDetector.OwnFaction = Faction;

			friendDetector.gameObject.SetActive(true);
			friendDetector.OwnFaction = Faction;
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			friendDetector.OnEntered = OnFriendEntered;
			friendDetector.OnExited = OnFriendExited;
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			if (BuildableState == BuildableState.Completed 
				&& _rigidBody.velocity.x == 0)
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
			Logging.Log(Tags.ATTACK_BOAT, "StopAttacking()");
			CancelInvoke("Attack");
		}

		private void OnEnemyEntered(FactionObject enemey)
		{
			Logging.Log(Tags.ATTACK_BOAT, "OnEnemyEntered()");

			_enemyUnit = enemey;
			StopMoving();
			StartAttacking();
		}

		private void StartAttacking()
		{
			Logging.Log(Tags.ATTACK_BOAT, "StartAttacking()");
			InvokeRepeating("Attack", _fireDelayInS, turretStats.FireIntervalInS);
		}

		private void Attack()
		{
			// FELIX Find angle instead of hardcoding
			float desiredAngle = 0;
			shellSpawner.SpawnShell(desiredAngle, facingDirection);
		}

		private void OnFriendEntered(FactionObject friend)
		{
			Logging.Log(Tags.ATTACK_BOAT, "OnFriendEntered()");

			if (IsObjectInFront(friend))
			{
				_blockingFriendlyUnit = friend;
				StopMoving();
			}
		}

		private void OnFriendExited(FactionObject friend)
		{
			Logging.Log(Tags.ATTACK_BOAT, "OnFriendExited()");

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
			Logging.Log(Tags.ATTACK_BOAT, "StartMoving()");
			_rigidBody.velocity = new Vector2(velocityInMPerS * _directionMultiplier, 0);
		}

		private void StopMoving()
		{
			Logging.Log(Tags.ATTACK_BOAT, "StopMoving()");
			_rigidBody.velocity = new Vector2(0, 0);
		}
	}
}
