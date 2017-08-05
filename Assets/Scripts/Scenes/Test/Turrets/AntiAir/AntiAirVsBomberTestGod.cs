using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.Defensive;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Turrets.AnitAir
{
    public class AntiAirVsBomberTestGod : MonoBehaviour 
	{
		private BomberController _bomber;
		private DefensiveTurret _antiAirTurret;

		public List<Vector2> bomberPatrolPoints;

		void Start() 
		{
			Helper helper = new Helper();


			// Set up turret
			_antiAirTurret = FindObjectOfType<DefensiveTurret>();
			Assert.IsNotNull(_antiAirTurret);

			helper.InitialiseBuildable(_antiAirTurret, faction: Faction.Reds);
			_antiAirTurret.StartConstruction();


			// Set up bomber
			_bomber = FindObjectOfType<BomberController>();
			Assert.IsNotNull(_bomber);

			ITargetFilter targetFilter = new FactionAndTargetTypeFilter(_antiAirTurret.Faction, _antiAirTurret.TargetType);
			ITargetsFactory targetsFactory = helper.CreateTargetsFactory(_antiAirTurret.GameObject, targetFilter);
			IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(bomberPatrolPoints: bomberPatrolPoints);
			helper.InitialiseBuildable(_bomber, faction: Faction.Blues, targetsFactory: targetsFactory, aircraftProvider: aircraftProvider);
			_bomber.StartConstruction();
		}
	}
}
