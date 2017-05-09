using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets;
using BattleCruisers.TestScenes.Utilities;
using BattleCruisers.Units.Aircraft;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes.Aircraft
{
	public class FighterTestsGod : MonoBehaviour 
	{
		public FighterController fighter;
		public GameObject target;
		public List<Vector2> patrolPoints;

		void Start() 
		{
			Helper helper = new Helper();
//			ITargetsFactory targetsFactory = helper.CreateTargetsFactory(target);

			helper.InitialiseBuildable(fighter, parentCruiserDirection: Direction.Right);
//			helper.InitialiseBuildable(fighter, targetsFactory: targetsFactory, parentCruiserDirection: Direction.Right);
			fighter.CompletedBuildable += Fighter_CompletedBuildable;
			fighter.StartConstruction();
		}

		private void Fighter_CompletedBuildable(object sender, EventArgs e)
		{
			FighterController fighter = sender as FighterController;
			fighter.PatrolPoints = patrolPoints;
			fighter.StartPatrolling();
		}			
	}
}
