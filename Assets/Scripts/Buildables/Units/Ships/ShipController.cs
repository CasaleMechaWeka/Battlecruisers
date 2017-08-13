using System.Collections.Generic;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    /// <summary>
    /// Assumptions:
    /// 1. Boats only move horizontally, and are all at the same height
    /// 2. All enemies will come towards the front of the boat, and all allies will come
    ///     towards the rear of the boat.
    /// 3. Boat will only stop to fight enemies.  Either this boat is destroyed, or the
    ///     enemy, in which case this boat will continue moving.
    /// </summary>
    public abstract class ShipController : Unit, ITargetConsumer
	{
		private int _directionMultiplier;
		private ITarget _blockingFriendlyUnit;
		private ITargetFinder _enemyFinder, _friendFinder;
        protected ITargetProcessor _enemyStoppingTargetProcessor;

		public CircleTargetDetector enemyDetector, friendDetector;

        protected abstract float EnemyDetectionRangeInM { get; }
		public override TargetType TargetType { get { return TargetType.Ships; } }

        private ITarget _blockingEnemyTarget;
        public ITarget Target 
        { 
            private get { return _blockingEnemyTarget; }
            set
            {
                if (value != null)
                {
                    Assert.IsTrue(IsObjectInFront(value));
                }

                _blockingEnemyTarget = value;
            }
        }

        public override void StaticInitialise()
		{
			base.StaticInitialise();

			_attackCapabilities.Add(TargetType.Ships);
			_attackCapabilities.Add(TargetType.Cruiser);
			_attackCapabilities.Add(TargetType.Buildings);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			_directionMultiplier = FacingDirection == Direction.Right ? 1 : -1;

            // Enemy detection for stopping (gnore aircraft for stopping)
			IList<TargetType> enemyTypesToStopFor = new List<TargetType>() { TargetType.Ships, TargetType.Cruiser, TargetType.Buildings };
            Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            bool isDetectable = true;
            enemyDetector.Initialise(EnemyDetectionRangeInM);
            ITargetFilter enemyDetectionFilter = _targetsFactory.CreateDetectableTargetFilter(enemyFaction, isDetectable, enemyTypesToStopFor);
			_enemyFinder = _targetsFactory.CreateRangedTargetFinder(enemyDetector, enemyDetectionFilter);
			
            ITargetRanker targetRanker = _targetsFactory.CreateEqualTargetRanker();
			_enemyStoppingTargetProcessor = _targetsFactory.CreateTargetProcessor(_enemyFinder, targetRanker);
			_enemyStoppingTargetProcessor.AddTargetConsumer(this);

			// Friend detection for stopping
			IList<TargetType> friendTargetTypes = new List<TargetType>() { TargetType.Ships };
			ITargetFilter friendFilter = _targetsFactory.CreateTargetFilter(Faction, friendTargetTypes);
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
			Logging.Verbose(Tags.ATTACK_BOAT, "rigidBody.velocity: " + rigidBody.velocity);
			rigidBody.velocity = new Vector2(maxVelocityInMPerS * _directionMultiplier, 0);
			Logging.Verbose(Tags.ATTACK_BOAT, "rigidBody.velocity: " + rigidBody.velocity);
		}

		private void StopMoving()
		{
			Logging.Log(Tags.ATTACK_BOAT, "StopMoving()");
			Logging.Verbose(Tags.ATTACK_BOAT, "rigidBody.velocity: " + rigidBody.velocity);
			rigidBody.velocity = new Vector2(0, 0);
			Logging.Verbose(Tags.ATTACK_BOAT, "rigidBody.velocity: " + rigidBody.velocity);
		}
	}
}
