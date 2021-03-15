using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Turrets
{
    public abstract class SmartMissileTestGod : TestGodBase
	{
		public SmartProjectileStats projectileStats;
		public TestTarget enemyCruiserTarget;
		public ShipController enemyShip;
		public TestAircraftController enemyAircraft;
		public Factory enemyFactory;
		public List<Vector2> aircraftPatrolPoints;

		protected override List<GameObject> GetGameObjects()
		{
			BCUtils.Helper.AssertIsNotNull(enemyAircraft, enemyShip, enemyFactory, enemyCruiserTarget, projectileStats);

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

			await InitialiseMissileAsync(helper, redCruiser);
		}

		protected abstract Task InitialiseMissileAsync(Helper helper, ICruiser redCruiser);
	}
}