using BattleCruisers.Units.Aircraft;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.TestScenes.Aircraft
{
	public class AircraftPatrollingTestsGod : MonoBehaviour 
	{
		public AircraftController aircraft;
		public List<GameObject> patrolPoints;

		void Start() 
		{
			IList<Vector3> patrolPointsAsVectors = patrolPoints.ConvertAll(gameObject => gameObject.transform.position);
			aircraft.PatrolPoints = patrolPointsAsVectors;

			aircraft.StartPatrolling();
		}
	}
}
