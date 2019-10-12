using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Projectiles.Spawners.Laser;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test
{
    public class LaserStats
    {
		public LaserEmitter Laser { get; }
		public float AngleInDegrees { get; }
		public bool IsSourceMirrored { get; }

		public LaserStats(LaserEmitter laser, float angleInDegrees, bool isSourceMirrored)
		{
			Laser = laser;
			AngleInDegrees = angleInDegrees;
			IsSourceMirrored = isSourceMirrored;
		}
    }

    public class LaserTest<T> where T : class, IBuildable
	{
        public LaserStats LaserStats { get; }
		public T Target { get; }

		public LaserTest(LaserEmitter laser, T target, float angleInDegrees, bool isSourceMirrored)
		{
            LaserStats = new LaserStats(laser, angleInDegrees, isSourceMirrored);
			Target = target;
		}
	}

	public class LaserEmitterTestGod : TestGodBase
	{
		private Helper _helper;
		private Faction _enemyFaction;
		private ISoundFetcher _soundFetcher;
        private IList<LaserTest<IBuilding>> _stationaryTargets;
        private IList<LaserTest<IUnit>> _movingTargets;

		public Building targetRightLevel, targetRightLevelBlockingEnemy, targetRightLevelBlockingFriendly, targetLeftLevel, targetLeftAngled, targetRightAngled;
		public TestAircraftController targetRightMoving, targetMovingLeft;
		public LaserEmitter laserEmitterRightLevel, laserEmitterLeftLevel, laserEmitterRightAngled, laserEmitterLeftAngled, laserEmitterLeftMoving, laserEmitterRightMoving;

		protected override void Start () 
		{
            base.Start();

            TimeScaleDeferrer timeScaleDeferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(timeScaleDeferrer);

			_helper = new Helper(updaterProvider: _updaterProvider, deferrer: timeScaleDeferrer);
			_enemyFaction = Faction.Blues;
			Faction friendlyFaction = Faction.Reds;
            _soundFetcher = new SoundFetcher();

			// Stationary targets
			_stationaryTargets = CreateStationaryTargetTests();
            foreach (LaserTest<IBuilding> test in _stationaryTargets)
            {
                _helper.InitialiseBuilding(test.Target, _enemyFaction);
                test.Target.StartConstruction();

                SetupLaser(test.LaserStats.Laser);
            }

            // Moving targets
            _movingTargets = CreateMovingTargetTests();

			foreach (LaserTest<IUnit> test in _movingTargets)
			{
                _helper.InitialiseUnit(test.Target, _enemyFaction);
                test.Target.StartConstruction();

                SetupLaser(test.LaserStats.Laser);
			}

			// Blocking targets
			_helper.InitialiseBuilding(targetRightLevelBlockingEnemy, _enemyFaction);
			targetRightLevelBlockingEnemy.StartConstruction();

			_helper.InitialiseBuilding(targetRightLevelBlockingFriendly, friendlyFaction);
			targetRightLevelBlockingFriendly.StartConstruction();


            _updaterProvider.BarrelControllerUpdater.Updated += BarrelControllerUpdater_Updated;
		}

        private IList<LaserTest<IBuilding>> CreateStationaryTargetTests()
        {
            IList<LaserTest<IBuilding>> stationaryTargets = new List<LaserTest<IBuilding>>();

			stationaryTargets.Add(new LaserTest<IBuilding>(laserEmitterLeftLevel, targetRightLevel, angleInDegrees: 0, isSourceMirrored: false));
			stationaryTargets.Add(new LaserTest<IBuilding>(laserEmitterRightLevel, targetLeftLevel, angleInDegrees: 0, isSourceMirrored: true));
			stationaryTargets.Add(new LaserTest<IBuilding>(laserEmitterRightAngled, targetLeftAngled, angleInDegrees: 45, isSourceMirrored: true));
			stationaryTargets.Add(new LaserTest<IBuilding>(laserEmitterLeftAngled, targetRightAngled, angleInDegrees: 45, isSourceMirrored: false));

            return stationaryTargets;
        }

        private IList<LaserTest<IUnit>> CreateMovingTargetTests()
        {
            IList<LaserTest<IUnit>> movingTargets = new List<LaserTest<IUnit>>();

			movingTargets.Add(new LaserTest<IUnit>(laserEmitterLeftMoving, targetRightMoving, angleInDegrees: 0, isSourceMirrored: false));
			SetupMovingTarget(targetRightMoving, isSourceMirrored: false);

			movingTargets.Add(new LaserTest<IUnit>(laserEmitterRightMoving, targetMovingLeft, angleInDegrees: 0, isSourceMirrored: true));
			SetupMovingTarget(targetMovingLeft, isSourceMirrored: true);

            return movingTargets;
        }

        private void SetupLaser(LaserEmitter laserEmitter)
        {
            IList<TargetType> targetTypes = new List<TargetType>() { TargetType.Buildings, TargetType.Cruiser };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(_enemyFaction, targetTypes);
            ITarget parent = Substitute.For<ITarget>();
            laserEmitter
                .InitialiseAsync(
                    targetFilter, 
                    damagePerS: 100, 
                    parent: parent, 
                    soundFetcher: _soundFetcher, 
                    deltaTimeProvider: _updaterProvider.BarrelControllerUpdater);
        }

        private void SetupMovingTarget(TestAircraftController movingTarget, bool isSourceMirrored)
		{
            // Pretend to be cruiser so laser will attack aircraft
			movingTarget.SetTargetType(TargetType.Cruiser);
			movingTarget.PatrolPoints = new List<Vector2>() 
			{
				movingTarget.Position,
				new Vector2(movingTarget.Position.x + (isSourceMirrored ? -2 : 2), movingTarget.Position.y)
			};
		}


        private void BarrelControllerUpdater_Updated(object sender, EventArgs e)
        {
            foreach (LaserTest<IBuilding> test in _stationaryTargets)
            {
                FireOrCease(test.LaserStats, test.Target);
            }

            foreach (LaserTest<IUnit> test in _movingTargets)
			{
				FireOrCease(test.LaserStats, test.Target);
			}
        }

        private static void FireOrCease(LaserStats stats, IBuildable target)
        {
            if (target.BuildableState == BuildableState.Completed && !target.IsDestroyed)
            {
                stats.Laser.FireLaser(stats.AngleInDegrees, stats.IsSourceMirrored);
            }
            else
            {
                stats.Laser.StopLaser();
            }
        }
    }
}
