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
		public FighterController fighter;
		public AircraftController targetAircraft;
		public List<Vector2> patrolPoints;

		void Start() 
		{
			ICruiser enemyCruiser = Substitute.For<ICruiser>();

			Helper helper = new Helper();
			ITargetsFactory targetsFactory = new TargetsFactory(enemyCruiser);

			helper.InitialiseBuildable(fighter, parentCruiserDirection: Direction.Right, faction: Faction.Reds, targetsFactory: targetsFactory);
			fighter.CompletedBuildable += Fighter_CompletedBuildable;
			fighter.StartConstruction();

			helper.InitialiseBuildable(targetAircraft);
			targetAircraft.StartConstruction();
		}

		private void Fighter_CompletedBuildable(object sender, EventArgs e)
		{
			FighterController fighter = sender as FighterController;
			fighter.PatrolPoints = patrolPoints;
			fighter.StartPatrolling();
		}			
	}
}
