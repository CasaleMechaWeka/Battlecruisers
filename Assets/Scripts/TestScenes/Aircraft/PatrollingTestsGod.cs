using BattleCruisers.Units.Aircraft;
using BattleCruisers.TestScenes.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.TestScenes.Aircraft
{
	public class PatrollingTestsGod : MonoBehaviour 
	{
		public AircraftController aircraft;
		public List<GameObject> patrolPoints;

		void Start() 
		{
			Helper helper = new Helper();

			helper.InitialiseBuildable(aircraft);
			aircraft.CompletedBuildable += Aircraft_CompletedBuildable;
			aircraft.StartConstruction();
		}

		private void Aircraft_CompletedBuildable(object sender, EventArgs e)
		{
			aircraft.CompletedBuildable -= Aircraft_CompletedBuildable;

			IList<Vector2> patrolPointsAsVectors = patrolPoints.ConvertAll(gameObject => new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
			aircraft.PatrolPoints = patrolPointsAsVectors;
			
			aircraft.StartPatrolling();
		}
	}
}
