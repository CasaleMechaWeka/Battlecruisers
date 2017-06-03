using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Scenes.Test.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
	public class PatrollingTestsGod : MonoBehaviour 
	{
		private AircraftController _aircraft;

		public List<GameObject> patrolPoints;

		void Start() 
		{
			Helper helper = new Helper();

			_aircraft = GameObject.FindObjectOfType<AircraftController>();
			helper.InitialiseBuildable(_aircraft);
			_aircraft.CompletedBuildable += Aircraft_CompletedBuildable;
			_aircraft.StartConstruction();
		}

		private void Aircraft_CompletedBuildable(object sender, EventArgs e)
		{
			_aircraft.CompletedBuildable -= Aircraft_CompletedBuildable;

			IList<Vector2> patrolPointsAsVectors = patrolPoints.ConvertAll(gameObject => new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
			_aircraft.PatrolPoints = patrolPointsAsVectors;
			
			_aircraft.StartPatrolling();
		}
	}
}
