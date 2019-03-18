using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Turrets.AnitAir
{
    public class AntiAirVsBomberTestGod : MonoBehaviour 
	{
		private BomberController _bomber;
		private TurretController _antiAirTurret;

		public List<Vector2> bomberPatrolPoints;

		void Start() 
		{
            IDeferrer deferrer = new Deferrer();
			Helper helper = new Helper(deferrer: deferrer);


			// Set up turret
			_antiAirTurret = FindObjectOfType<TurretController>();
			Assert.IsNotNull(_antiAirTurret);

            helper.InitialiseBuilding(_antiAirTurret, faction: Faction.Reds);
			_antiAirTurret.StartConstruction();


			// Set up bomber
			_bomber = FindObjectOfType<BomberController>();
			Assert.IsNotNull(_bomber);

            IList<TargetType> targetTypes = new List<TargetType>() { _antiAirTurret.TargetType };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(_antiAirTurret.Faction, targetTypes);
            ITargetFactoriesProvider targetFactories = helper.CreateTargetFactories(_antiAirTurret.GameObject, targetFilter);
			IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(bomberPatrolPoints: bomberPatrolPoints);
            helper.InitialiseUnit(_bomber, faction: Faction.Blues, targetFactories: targetFactories, aircraftProvider: aircraftProvider);
			_bomber.StartConstruction();
		}
	}
}
