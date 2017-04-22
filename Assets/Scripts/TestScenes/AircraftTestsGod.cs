using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes
{
	public class AircraftTestsGod : MonoBehaviour 
	{
		public BomberController bomber;

		void Start() 
		{
			Logging.Initialise();

			IList<Vector3> patrolPoints = new List<Vector3> { new Vector3(-5, 3, 0), new Vector3(5, 3, 0) };
			bomber.Initialise(patrolPoints);

			bomber.TempStartPatrolling();
		}
	}
}
