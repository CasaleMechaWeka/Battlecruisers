using BattleCruisers.Units.Aircraft;
using BattleCruisers.Utils;
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
			IList<Vector2> patrolPointsAsVectors = patrolPoints.ConvertAll(gameObject => new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
			aircraft.PatrolPoints = patrolPointsAsVectors;

			aircraft.StartPatrolling();
		}

		private void StopPatrolling()
		{
			Debug.Log("StopPatrolling");
			aircraft.StopPatrolling();
		}

		private void StartPatrolling()
		{
			aircraft.StartPatrolling();
		}
	}
}
