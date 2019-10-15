using System.Collections.Generic;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
    public class PatrollingTestsGod : TestGodBase
	{
        private TestAircraftController _aircraft;

        public List<GameObject> patrolPoints;

        protected override IList<GameObject> GetGameObjects()
        {
			_aircraft = FindObjectOfType<TestAircraftController>();

            return new List<GameObject>()
            {
                _aircraft.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
			IList<Vector2> patrolPointsAsVectors = patrolPoints.ConvertAll(gameObject => new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
			_aircraft.PatrolPoints = patrolPointsAsVectors;
            helper.InitialiseUnit(_aircraft);
			_aircraft.StartConstruction();
		}
	}
}
