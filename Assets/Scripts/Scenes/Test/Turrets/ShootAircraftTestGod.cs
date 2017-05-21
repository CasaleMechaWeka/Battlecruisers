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

namespace BattleCruisers.Scenes.Test.Turrets
{
	public class ShootAircraftTestGod : MonoBehaviour 
	{
		public BomberController bomber;
		public List<Vector2> bomberPatrolPoints;
		public DefensiveTurret turret;

		void Start() 
		{
			Helper helper = new Helper();

			// Set up turret
			helper.InitialiseBuildable(turret, faction: Faction.Reds);
			turret.StartConstruction();

			// Set up bomber
			ITargetsFactory targetsFactory = helper.CreateTargetsFactory(turret.GameObject);
			IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(bomberPatrolPoints: bomberPatrolPoints);
			helper.InitialiseBuildable(bomber, faction: Faction.Blues, targetsFactory: targetsFactory, aircraftProvider: aircraftProvider);
			bomber.StartConstruction();
		}
	}
}
