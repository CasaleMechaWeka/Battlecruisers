using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.UI.BattleScene;
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
		private ShellTurretBarrelController _turretBarrelController;
		private int _directionMultiplier;
		private ITarget _blockingFriendlyUnit;
		private ITargetFinder _enemyFinder, _friendFinder;
		private ITargetProcessor _targetProcessor;

		public TargetDetector enemyDetector, friendDetector;

		public override float Damage 
		{ 
			get 
			{ 
				return _turretBarrelController.TurretStats.DamagePerS; 
			} 
		}

		public ITarget Target
		{
			private get { return _turretBarrelController.Target; }
			set 
			{ 
				if (value != null)
				{
					Assert.IsTrue(IsObjectInFront(value));
				}

				_turretBarrelController.Target = value; 
			}
		}

		public override TargetType TargetType { get { return TargetType.Ships; } }

		protected override void OnAwake()
		{
			base.OnAwake();

			_turretBarrelController = gameObject.GetComponentInChildren<ShellTurretBarrelController>();
			Assert.IsNotNull(_turretBarrelController);
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			_attackCapabilities.Add(TargetType.Ships);
			_attackCapabilities.Add(TargetType.Cruiser);
			_attackCapabilities.Add(TargetType.Buildings);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			_directionMultiplier = FacingDirection == Direction.Right ? 1 : -1;

			IAngleCalculator angleCalculator = _angleCalculatorFactory.CreateAngleCalcultor(_targetPositionPredictorFactory);
			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
			ITargetFilter enemyFilter = _targetsFactory.CreateTargetFilter(enemyFaction, _attackCapabilities);
			_turretBarrelController.Initialise(enemyFilter, angleCalculator);

			// Enemy detection
			enemyDetector.Initialise(_turretBarrelController.TurretStats.rangeInM);
			_enemyFinder = _targetsFactory.CreateRangedTargetFinder(enemyDetector, enemyFilter);

			ITargetRanker targetRanker = _targetsFactory.CreateEqualTargetRanker();
			_targetProcessor = _targetsFactory.CreateTargetProcessor(_enemyFinder, targetRanker);
			_targetProcessor.AddTargetConsumer(this);

			// Friend detection
			ITargetFilter friendFilter = _targetsFactory.CreateTargetFilter(Faction, TargetType.Ships);
			_friendFinder = _targetsFactory.CreateRangedTargetFinder(friendDetector, friendFilter);
			_friendFinder.TargetFound += OnFriendFound;
			_friendFinder.TargetLost += OnFriendLost;
			_friendFinder.StartFindingTargets();
		}

		protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();

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

		private void OnFriendFound(object sender, TargetEventArgs args)
		{
			Logging.Log(Tags.ATTACK_BOAT, "OnFriendFound()");

			if (IsObjectInFront(args.Target))
			{
				_blockingFriendlyUnit = args.Target;
			}
		}

		private void OnFriendLost(object sender, TargetEventArgs args)
		{
			Logging.Log(Tags.ATTACK_BOAT, "OnFriendLost()");

			if (IsObjectInFront(args.Target))
			{
				Assert.IsTrue(_blockingFriendlyUnit != null);

				if (object.ReferenceEquals(_blockingFriendlyUnit, args.Target))
				{
					_blockingFriendlyUnit = null;
				}
			}
		}

		private bool IsObjectInFront(ITarget target)
		{
			return (FacingDirection == Direction.Right
					&& target.Position.x > transform.position.x)
				|| (FacingDirection == Direction.Left
					&& target.Position.x < transform.position.x);
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
