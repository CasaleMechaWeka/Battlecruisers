using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.UI;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// FELIX  Create parent boat class
// FELIX  Create Naval folder & namespace
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
	public class AttackBoatController : Unit, ITargetConsumer
	{
		private int _directionMultiplier;
		private ITarget _blockingFriendlyUnit;
		private ITargetFinder _targetFinder;
		private ITargetProcessor _targetProcessor;

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

		public ITarget Target
		{
			private get { return turretBarrelController.Target; }
			set 
			{ 
				if (value != null)
				{
					Assert.IsTrue(IsObjectInFront(value));
				}

				turretBarrelController.Target = value; 
			}
		}

		public override TargetType TargetType { get { return TargetType.Ships; } }

		protected override void OnInitialised()
		{
			base.OnInitialised();

			_attackCapabilities.Add(TargetType.Ships);
			_attackCapabilities.Add(TargetType.Cruiser);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			_directionMultiplier = facingDirection == Direction.Right ? 1 : -1;
			
			turretBarrelController.Initialise(Faction);

			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
			ITargetFilter enemyFilter = _targetsFactory.CreateTargetFilter(enemyFaction, TargetType.Ships, TargetType.Buildings, TargetType.Cruiser);
			enemyDetector.Initialise(enemyFilter, turretBarrelController.turretStats.rangeInM);

			_targetFinder = _targetsFactory.CreateRangedTargetFinder(enemyDetector);
			ITargetRanker targetRanker = _targetsFactory.CreateEqualTargetRanker();
			_targetProcessor = _targetsFactory.CreateTargetProcessor(_targetFinder, targetRanker);
			_targetProcessor.AddTargetConsumer(this);

			ITargetFilter friendFilter = _targetsFactory.CreateTargetFilter(Faction, TargetType.Ships);
			friendDetector.Initialise(friendFilter);
			friendDetector.OnEntered += OnFriendEntered;
			friendDetector.OnExited += OnFriendExited;
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			if (BuildableState == BuildableState.Completed)
			{
				if (rigidBody.velocity.x == 0)
				{
					if (Target == null
					    && _blockingFriendlyUnit == null)
					{
						StartMoving();
					}
				}
				else if (Target != null
			         || _blockingFriendlyUnit != null)
				{
					StopMoving();
				}
			}
		}

		private void OnFriendEntered(object sender, TargetEventArgs args)
		{
			Logging.Log(Tags.ATTACK_BOAT, "OnFriendEntered()");

			if (IsObjectInFront(args.Target))
			{
				_blockingFriendlyUnit = args.Target;
				_blockingFriendlyUnit.Destroyed += BlockingFriendlyUnit_Destroyed;
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
