using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Turrets.AnitAir
{
    public class AntiAirVsBomberTestGod : TestGodBase 
	{
		private BomberController _bomber;
		private TurretController _antiAirTurret;

		public List<Vector2> bomberPatrolPoints;

        protected async override Task<Helper> CreateHelperAsync(IUpdaterProvider updaterProvider)
        {
            IDeferrer deferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(deferrer);

            return await HelperFactory.CreateHelperAsync(deferrer: deferrer, updaterProvider: updaterProvider);
        }

        protected override List<GameObject> GetGameObjects()
        {
			_antiAirTurret = FindObjectOfType<TurretController>();
			Assert.IsNotNull(_antiAirTurret);

            _bomber = FindObjectOfType<BomberController>();
			Assert.IsNotNull(_bomber);

            return new List<GameObject>()
            {
                _antiAirTurret.GameObject,
                _bomber.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
			// Set up turret
            helper.InitialiseBuilding(_antiAirTurret, faction: Faction.Reds);
			_antiAirTurret.StartConstruction();

			// Set up bomber
            IList<TargetType> targetTypes = new List<TargetType>() { _antiAirTurret.TargetType };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(_antiAirTurret.Faction, targetTypes);
            ITargetFactories targetFactories = helper.CreateTargetFactories(_antiAirTurret.GameObject, targetFilter: targetFilter);
			IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(bomberPatrolPoints: bomberPatrolPoints);
            helper.InitialiseUnit(_bomber, faction: Faction.Blues, targetFactories: targetFactories, aircraftProvider: aircraftProvider);
			_bomber.StartConstruction();
		}
	}
}
