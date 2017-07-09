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

		public Buildable targetRightLevel, targetLeftLevel, targetLeftAngled, targetRightAngled;
		public LaserEmitter laserEmitterRightLevel, laserEmitterLeftLevel, laserEmitterRightAngled, laserEmitterLeftAngled;

		void Start () 
		{
			_helper = new Helper();
			_enemyFaction = Faction.Blues;
			_laserTests = new List<LaserTest>();

			_laserTests.Add(new LaserTest(laserEmitterLeftLevel, targetRightLevel, angleInDegrees: 0, isSourceMirrored: false));
			_laserTests.Add(new LaserTest(laserEmitterRightLevel, targetLeftLevel, angleInDegrees: 0, isSourceMirrored: true));
			_laserTests.Add(new LaserTest(laserEmitterRightAngled, targetLeftAngled, angleInDegrees: 45, isSourceMirrored: true));
			_laserTests.Add(new LaserTest(laserEmitterLeftAngled, targetRightAngled, angleInDegrees: 45, isSourceMirrored: false));

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
