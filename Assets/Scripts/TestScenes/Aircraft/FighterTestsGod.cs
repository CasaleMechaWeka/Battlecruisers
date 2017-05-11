using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets;
using BattleCruisers.TestScenes.Utilities;
using BattleCruisers.Units.Aircraft;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes.Aircraft
{
	public class FighterTestsGod : MonoBehaviour 
	{
		public FighterController fighter1, fighter2, fighter3;
		public AircraftController targetAircraft1, targetAircraft2, targetAircraft3;
		public List<Vector2> patrolPoints;

		void Start() 
		{
			Helper helper = new Helper();


			// Stationary target
			ITargetsFactory targetsFactory1 = helper.CreateTargetsFactory(targetAircraft1.GameObject);

			helper.InitialiseBuildable(fighter1, faction: Faction.Reds, targetsFactory: targetsFactory1);
			fighter1.CompletedBuildable += Fighter_CompletedBuildable;
			fighter1.StartConstruction();

			helper.InitialiseBuildable(targetAircraft1, faction: Faction.Blues);
			targetAircraft1.StartConstruction();


//			// Patrolling target
//			ITargetsFactory targetsFactory2 = helper.CreateTargetsFactory(targetAircraft1.GameObject);
//
//			helper.InitialiseBuildable(fighter2, parentCruiserDirection: Direction.Right, faction: Faction.Reds, targetsFactory: targetsFactory1);
//			fighter1.CompletedBuildable += Fighter_CompletedBuildable;
//			fighter1.StartConstruction();
//
//			helper.InitialiseBuildable(targetAircraft1);
//			targetAircraft1.StartConstruction();


			// Dogfight
		}

		private void Fighter_CompletedBuildable(object sender, EventArgs e)
		{
//			FighterController fighter = sender as FighterController;
//			fighter.PatrolPoints = patrolPoints;
//			fighter.StartPatrolling();

//			fighter.Target = targetAircraft;
		}			
	}
}
