using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
	public class LaserTest
	{
		public LaserEmitter Laser { get; private set; }
		public Buildable Target { get; private set; }
		public float AngleInDegrees { get; private set; }
		public bool IsSourceMirrored { get; private set; }

		public LaserTest(LaserEmitter laser, Buildable target, float angleInDegrees, bool isSourceMirrored)
		{
			Laser = laser;
			Target = target;
			AngleInDegrees = angleInDegrees;
			IsSourceMirrored = isSourceMirrored;
		}
	}

	public class LaserEmitterTestGod : MonoBehaviour 
	{
		private Helper _helper;
		private Faction _enemyFaction;
		private IList<LaserTest> _laserTests;

		public Buildable targetRightLevel, targetRightLevelBlockingEnemy, targetRightLevelBlockingFriendly, targetLeftLevel, targetLeftAngled, targetRightAngled;
		public TestAircraftController targetRightMoving, targetMovingLeft;
		public LaserEmitter laserEmitterRightLevel, laserEmitterLeftLevel, laserEmitterRightAngled, laserEmitterLeftAngled, laserEmitterLeftMoving, laserEmitterRightMoving;

		void Start () 
		{
			_helper = new Helper();
			_enemyFaction = Faction.Blues;
			Faction friendlyFaction = Faction.Reds;
			_laserTests = new List<LaserTest>();

			_laserTests.Add(new LaserTest(laserEmitterLeftLevel, targetRightLevel, angleInDegrees: 0, isSourceMirrored: false));
			_laserTests.Add(new LaserTest(laserEmitterRightLevel, targetLeftLevel, angleInDegrees: 0, isSourceMirrored: true));
			_laserTests.Add(new LaserTest(laserEmitterRightAngled, targetLeftAngled, angleInDegrees: 45, isSourceMirrored: true));
			_laserTests.Add(new LaserTest(laserEmitterLeftAngled, targetRightAngled, angleInDegrees: 45, isSourceMirrored: false));

			_laserTests.Add(new LaserTest(laserEmitterLeftMoving, targetRightMoving, angleInDegrees: 0, isSourceMirrored: false));
			SetupMovingTarget(targetRightMoving, isSourceMirrored: false);

			_laserTests.Add(new LaserTest(laserEmitterRightMoving, targetMovingLeft, angleInDegrees: 0, isSourceMirrored: true));
			SetupMovingTarget(targetMovingLeft, isSourceMirrored: true);

			_helper.InitialiseBuildable(targetRightLevelBlockingEnemy, _enemyFaction);
			targetRightLevelBlockingEnemy.StartConstruction();
			
			_helper.InitialiseBuildable(targetRightLevelBlockingFriendly, friendlyFaction);
			targetRightLevelBlockingFriendly.StartConstruction();

			foreach (LaserTest test in _laserTests)
			{
				SetupPair(test.Laser, test.Target);
			}
		}

		private void SetupPair(LaserEmitter laserEmitter, Buildable target)
		{
			// Setup target
			_helper.InitialiseBuildable(target, _enemyFaction);
			target.StartConstruction();
			
			// Setup laser
			ITargetFilter targetFilter = new FactionAndTargetTypeFilter(_enemyFaction, TargetType.Buildings, TargetType.Cruiser);
			laserEmitter.Initialise(targetFilter, damagePerS: 100);
		}

		private void SetupMovingTarget(TestAircraftController movingTarget, bool isSourceMirrored)
		{
			movingTarget.SetTargetType(TargetType.Cruiser);
			movingTarget.PatrolPoints = new List<Vector2>() 
			{
				movingTarget.Position,
				new Vector2(movingTarget.Position.x + (isSourceMirrored ? -2 : 2), movingTarget.Position.y)
			};
		}

		void Update()
		{
			foreach (LaserTest test in _laserTests)
			{
				if (!test.Target.IsDestroyed)
				{
					test.Laser.FireLaser(test.AngleInDegrees, test.IsSourceMirrored);
				}
				else
				{
					test.Laser.StopLaser();
				}
			}
		}
	}
}
