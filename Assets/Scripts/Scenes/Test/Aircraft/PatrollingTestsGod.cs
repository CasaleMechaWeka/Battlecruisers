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
		public List<GameObject> patrolPoints;

		void Start() 
		{
			Helper helper = new Helper();

			AircraftController aircraft = GameObject.FindObjectOfType<AircraftController>();
			IList<Vector2> patrolPointsAsVectors = patrolPoints.ConvertAll(gameObject => new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
			aircraft.PatrolPoints = patrolPointsAsVectors;
			helper.InitialiseBuildable(aircraft);
			aircraft.StartConstruction();
		}
	}
}
