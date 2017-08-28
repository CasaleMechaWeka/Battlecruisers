using System.Collections.Generic;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
    public class PatrollingTestsGod : MonoBehaviour 
	{
		public List<GameObject> patrolPoints;

		void Start() 
		{
			Helper helper = new Helper();

			TestAircraftController aircraft = FindObjectOfType<TestAircraftController>();
			IList<Vector2> patrolPointsAsVectors = patrolPoints.ConvertAll(gameObject => new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
			aircraft.PatrolPoints = patrolPointsAsVectors;
			helper.InitialiseBuildable(aircraft);
			aircraft.StartConstruction();
		}
	}
}
