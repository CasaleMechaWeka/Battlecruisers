using System.Collections.Generic;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
    public class AircraftDestroyedTestGod : MonoBehaviour 
	{
        private TestAircraftController _aircraft;
		public List<Vector2> patrolPoints;

		void Start() 
		{
			Helper helper = new Helper();

			_aircraft = FindObjectOfType<TestAircraftController>();
			_aircraft.PatrolPoints = patrolPoints;
            helper.InitialiseUnit(_aircraft);
			_aircraft.StartConstruction();

            Invoke("DestroyAircraft", time: 1);
		}

        private void DestroyAircraft()
        {
            _aircraft.TakeDamage(damageAmount: _aircraft.MaxHealth, damageSource: null);
        }
	}
}
