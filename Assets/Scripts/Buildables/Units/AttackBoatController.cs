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
	{
		private Rigidbody2D _rigidBody;
		private int _directionMultiplier;
		private FactionObject _blockingFriendlyUnit;
		
		public FactionObjectDetector enemyDetector;
		public FactionObjectDetector friendDetector;
		public TurretBarrelController turretBarrelController;

		public override float Damage 
		{ 
			get 
			{ 
				return turretBarrelController.turretStats.DamagePerS; 
			} 
		}

		private FactionObject _enemyUnit;
		private FactionObject EnemyUnit
		{
			get { return _enemyUnit; }
			set
			{
				_enemyUnit = value;
				turretBarrelController.Target = _enemyUnit != null ? _enemyUnit.gameObject : null;
			}
		}

		void Start() 
		{
			_rigidBody = GetComponent<Rigidbody2D>();
			_directionMultiplier = facingDirection == Direction.Right ? 1 : -1;

			enemyDetector.Initialise(Helper.GetOppositeFaction(Faction));
			enemyDetector.OnEntered = OnEnemyEntered;

			friendDetector.Initialise(Faction);
			friendDetector.gameObject.SetActive(true);
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
				&& _rigidBody.velocity.x == 0
				&& EnemyUnit == null
				&& _blockingFriendlyUnit == null)
			{
				StartMoving();
			}
		}

		private void OnEnemyEntered(FactionObject enemy)
		{
			Logging.Log(Tags.ATTACK_BOAT, "OnEnemyEntered()");

			EnemyUnit = enemy;
			EnemyUnit.Destroyed += EnemyUnit_Destroyed;
			StopMoving();
		}

		// FELIX  Attack other in range unit?
		private void EnemyUnit_Destroyed(object sender, EventArgs e)
		{
			EnemyUnit.Destroyed -= EnemyUnit_Destroyed;
			EnemyUnit = null;
		}

		private void OnFriendEntered(FactionObject friend)
		{
			Logging.Log(Tags.ATTACK_BOAT, "OnFriendEntered()");

			if (IsObjectInFront(friend))
			{
				_blockingFriendlyUnit = friend;
				_blockingFriendlyUnit.Destroyed += BlockingFriendlyUnit_Destroyed;
				StopMoving();
			}
		}

		private void BlockingFriendlyUnit_Destroyed(object sender, EventArgs e)
		{
			_blockingFriendlyUnit.Destroyed -= BlockingFriendlyUnit_Destroyed;
			_blockingFriendlyUnit = null;
		}

		private void OnFriendExited(FactionObject friend)
		{
			Logging.Log(Tags.ATTACK_BOAT, "OnFriendExited()");

			if (IsObjectInFront(friend))
			{
				_blockingFriendlyUnit.Destroyed -= BlockingFriendlyUnit_Destroyed;
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
