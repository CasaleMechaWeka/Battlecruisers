using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
    public class KamikazeTestGod : MonoBehaviour
	{
        private IFactory _target;
        private AircraftController[] _aircraft;

		public List<Vector2> bomberPatrolPoints;

		void Start()
		{
			Helper helper = new Helper();
			
            // Setup target
			_target = FindObjectOfType<Factory>();
			helper.InitialiseBuildable(_target);
			_target.StartConstruction();

            // Setup AA
            TurretController aaTurret = FindObjectOfType<TurretController>();
            helper.InitialiseBuildable(aaTurret);
            aaTurret.StartConstruction();

            // Setup aircraft
            _aircraft = FindObjectsOfType<AircraftController>();
            foreach (AircraftController aircraft in _aircraft)
            {
                helper.InitialiseBuildable(aircraft);
                aircraft.StartConstruction();
            }

			// When completed, aircraft switches to patrol movement controller.
			// Hence wait a bit after completed before setting kamikaze
			// homing movement controller.
			//_aircraft.CompletedBuildable += (sender, e) => Invoke("Kamikaze", time: 0.1f);
			Invoke("Kamikaze", time: 1);
		}

        public void Kamikaze()
        {
            foreach (AircraftController aircraft in _aircraft)
            {
                aircraft.Kamikaze(_target);
            }
        }
	}
}
