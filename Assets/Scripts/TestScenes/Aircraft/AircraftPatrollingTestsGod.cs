using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.TestScenes.Aircraft
{
	public class AircraftPatrollingTestsGod : MonoBehaviour 
	{
		public BomberController bomber;
		public List<GameObject> patrolPoints;

		void Start() 
		{
			IList<Vector3> patrolPointsAsVectors = patrolPoints.ConvertAll(gameObject => gameObject.transform.position);
			bomber.PatrolPoints = patrolPointsAsVectors;

			bomber.StartPatrolling();
		}
	}
}
