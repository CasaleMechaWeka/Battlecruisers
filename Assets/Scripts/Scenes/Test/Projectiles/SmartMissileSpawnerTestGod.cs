using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Static;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test
{
    public class SmartMissileSpawnerTestGod : TestGodBase
	{
		private ITargetFilter _targetFilter;

		public SmartMissileSpawner missileSpawner;
		public SmartProjectileStats projectileStats;
		public MissileController missilePrefab;
		// FELIX  Hmm..
		//public TestTarget enemyCruiser;
		public ShipController enemyShip;
		public TestAircraftController enemyAircraft;
		public List<Vector2> aircraftPatrolPoints;

        protected override List<GameObject> GetGameObjects()
        {
			BCUtils.Helper.AssertIsNotNull(enemyAircraft, enemyShip, missileSpawner, projectileStats, missilePrefab);

            return new List<GameObject>()
            {
                enemyAircraft.GameObject,
				enemyShip.GameObject
            };
        }

        protected override async Task SetupAsync(Helper helper)
        {
			ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);

			// Setup enemies
			enemyAircraft.PatrolPoints = aircraftPatrolPoints;
            helper.InitialiseUnit(enemyAircraft, Faction.Reds);
            enemyAircraft.StartConstruction();
			Helper.SetupUnitForUnitMonitor(enemyAircraft, redCruiser);

			helper.InitialiseUnit(enemyShip, Faction.Reds);
			enemyShip.StartConstruction();
			Helper.SetupUnitForUnitMonitor(enemyShip, redCruiser);


			// Setup missile spawner
			_targetFilter = new FactionAndTargetTypeFilter(Faction.Reds, projectileStats.AttackCapabilities);

			ITarget parent = Substitute.For<ITarget>();
            int burstSize = 1;
			BuildableInitialisationArgs args = helper.CreateBuildableInitialisationArgs(enemyCruiser: redCruiser);
            IProjectileSpawnerArgs spawnerArgs = new ProjectileSpawnerArgs(parent, projectileStats, burstSize, args.FactoryProvider, args.CruiserSpecificFactories, args.EnemyCruiser);

            await missileSpawner.InitialiseAsync(spawnerArgs, SoundKeys.Firing.Missile, projectileStats);

			InvokeRepeating("FireMissile", time: 0.5f, repeatRate: 0.5f);
		}

		private void FireMissile()
		{
			missileSpawner.SpawnMissile(angleInDegrees: 90, isSourceMirrored: false, _targetFilter);
		}
	}
}
