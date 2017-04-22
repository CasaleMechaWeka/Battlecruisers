using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.TestScenes
{
	public class AircraftTestsGod : MonoBehaviour 
	{
		public BomberController bomber;
		public List<GameObject> patrolPoints;

		void Start() 
		{
			Logging.Initialise();

			IList<Vector3> patrolPointsAsVectors = patrolPoints.ConvertAll(gameObject => gameObject.transform.position);
			bomber.Initialise(patrolPointsAsVectors);

			bomber.TempStartPatrolling();
		}
	}
}
