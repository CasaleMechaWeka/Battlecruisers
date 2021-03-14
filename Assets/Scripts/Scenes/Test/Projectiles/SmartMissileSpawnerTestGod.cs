using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Static;
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
        public TestTarget enemyCruiserTarget;
        public ShipController enemyShip;
		public TestAircraftController enemyAircraft;
		public Factory enemyFactory;
		public List<Vector2> aircraftPatrolPoints;

        protected override List<GameObject> GetGameObjects()
        {
			BCUtils.Helper.AssertIsNotNull(enemyAircraft, enemyShip, enemyFactory, enemyCruiserTarget, missileSpawner, projectileStats);

            return new List<GameObject>()
            {
                enemyAircraft.GameObject,
				enemyShip.GameObject,
				enemyFactory.GameObject,
				enemyCruiserTarget.GameObject
            };
        }

        protected override async Task SetupAsync(Helper helper)
        {
			ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);
			
			// Setup missile spawner
			_targetFilter = new FactionAndTargetTypeFilter(Faction.Reds, projectileStats.AttackCapabilities);

			ITarget parent = Substitute.For<ITarget>();
            int burstSize = 1;
			BuildableInitialisationArgs args = helper.CreateBuildableInitialisationArgs(enemyCruiser: redCruiser);
            IProjectileSpawnerArgs spawnerArgs = new ProjectileSpawnerArgs(parent, projectileStats, burstSize, args.FactoryProvider, args.CruiserSpecificFactories, args.EnemyCruiser);

            await missileSpawner.InitialiseAsync(spawnerArgs, SoundKeys.Firing.Missile, projectileStats);

			InvokeRepeating("FireMissile", time: 0.5f, repeatRate: 2);

			// Setup enemies
			enemyAircraft.PatrolPoints = aircraftPatrolPoints;
            helper.InitialiseUnit(enemyAircraft, Faction.Reds);
            enemyAircraft.StartConstruction();
			Helper.SetupUnitForUnitMonitor(enemyAircraft, redCruiser);

            helper.InitialiseUnit(enemyShip, Faction.Reds, parentCruiserDirection: Direction.Left);
            enemyShip.StartConstruction();
            Helper.SetupUnitForUnitMonitor(enemyShip, redCruiser);

			helper.InitialiseBuilding(enemyFactory, Faction.Reds);
			enemyFactory.StartConstruction();

			enemyCruiserTarget.Initialise(helper.CommonStrings, Faction.Reds);
        }

		private void FireMissile()
		{
			missileSpawner.SpawnMissile(angleInDegrees: 90, isSourceMirrored: false, _targetFilter);
		}
	}
}
