using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Units.Aircraft;
using BattleCruisers.Units.Aircraft.Providers;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
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
			_antiAirTurret = GameObject.FindObjectOfType<DefensiveTurret>();
			Assert.IsNotNull(_antiAirTurret);

			helper.InitialiseBuildable(_antiAirTurret, faction: Faction.Reds);
			_antiAirTurret.StartConstruction();


			// Set up bomber
			_bomber = GameObject.FindObjectOfType<BomberController>();
			Assert.IsNotNull(_bomber);

			ITargetsFactory targetsFactory = helper.CreateTargetsFactory(_antiAirTurret.GameObject);
			IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(bomberPatrolPoints: bomberPatrolPoints);
			helper.InitialiseBuildable(_bomber, faction: Faction.Blues, targetsFactory: targetsFactory, aircraftProvider: aircraftProvider);
			_bomber.StartConstruction();
		}
	}
}
