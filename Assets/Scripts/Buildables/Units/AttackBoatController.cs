using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
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
// FELIX  Create Naval folde & namespace
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
		private int _directionMultiplier;
		private ITarget _blockingFriendlyUnit;
		
		public TargetDetector enemyDetector;
		public TargetDetector friendDetector;
		public TurretBarrelController turretBarrelController;

		public override float Damage 
		{ 
			get 
			{ 
				return turretBarrelController.turretStats.DamagePerS; 
			} 
		}

		private ITarget _enemyUnit;
		private ITarget EnemyUnit
		{
			get { return _enemyUnit; }
			set
			{
				_enemyUnit = value;
				turretBarrelController.Target = _enemyUnit;
			}
		}

		public override TargetType TargetType { get { return TargetType.Ships; } }

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			_directionMultiplier = facingDirection == Direction.Right ? 1 : -1;
			
			turretBarrelController.Initialise(Faction);

			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
			ITargetFilter enemyFilter = _targetsFactory.CreateTargetFilter(enemyFaction, TargetType.Ships, TargetType.Buildings, TargetType.Cruiser);
			enemyDetector.Initialise(enemyFilter);
			enemyDetector.OnEntered += OnEnemyEntered;

			ITargetFilter friendFilter = _targetsFactory.CreateTargetFilter(Faction, TargetType.Ships);
			friendDetector.Initialise(friendFilter);
			friendDetector.OnEntered += OnFriendEntered;
			friendDetector.OnExited += OnFriendExited;
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			if (BuildableState == BuildableState.Completed 
				&& rigidBody.velocity.x == 0
				&& EnemyUnit == null
				&& _blockingFriendlyUnit == null)
			{
				StartMoving();
			}
		}

		private void OnEnemyEntered(object sender, TargetEventArgs args)
		{
			Logging.Log(Tags.ATTACK_BOAT, "OnEnemyEntered()");

			EnemyUnit = args.Target;
			EnemyUnit.Destroyed += EnemyUnit_Destroyed;
			StopMoving();
		}

		// FELIX  Attack other in range unit?
		private void EnemyUnit_Destroyed(object sender, EventArgs e)
		{
			EnemyUnit.Destroyed -= EnemyUnit_Destroyed;
			EnemyUnit = null;
		}

		private void OnFriendEntered(object sender, TargetEventArgs args)
		{
			Logging.Log(Tags.ATTACK_BOAT, "OnFriendEntered()");

			if (IsObjectInFront(args.Target))
			{
				_blockingFriendlyUnit = args.Target;
				_blockingFriendlyUnit.Destroyed += BlockingFriendlyUnit_Destroyed;
				StopMoving();
			}
		}

		private void BlockingFriendlyUnit_Destroyed(object sender, EventArgs e)
		{
			_blockingFriendlyUnit.Destroyed -= BlockingFriendlyUnit_Destroyed;
			_blockingFriendlyUnit = null;
		}

		private void OnFriendExited(object sender, TargetEventArgs args)
		{
			Logging.Log(Tags.ATTACK_BOAT, "OnFriendExited()");

			if (IsObjectInFront(args.Target))
			{
				_blockingFriendlyUnit.Destroyed -= BlockingFriendlyUnit_Destroyed;
				StartMoving();
			}
		}

		private bool IsObjectInFront(ITarget target)
		{
			return (facingDirection == Direction.Right
					&& target.GameObject.transform.position.x > transform.position.x)
				|| (facingDirection == Direction.Left
					&& target.GameObject.transform.position.x < transform.position.x);
		}

		private void StartMoving()
		{
			Logging.Log(Tags.ATTACK_BOAT, "StartMoving()");
			rigidBody.velocity = new Vector2(maxVelocityInMPerS * _directionMultiplier, 0);
		}

		private void StopMoving()
		{
			Logging.Log(Tags.ATTACK_BOAT, "StopMoving()");
			rigidBody.velocity = new Vector2(0, 0);
		}
	}
}
