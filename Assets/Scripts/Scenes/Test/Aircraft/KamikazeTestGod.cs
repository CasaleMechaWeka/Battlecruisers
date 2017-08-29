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

        // FELIX  Ues array of AircraftController?
        private TestAircraftController _aircraft;
        private BomberController _bomber;
        private FighterController _fighter;

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

            // Setup test aircraft
            _aircraft = FindObjectOfType<TestAircraftController>();
            helper.InitialiseBuildable(_aircraft);
            _aircraft.StartConstruction();

            // Setup bomber
			_bomber = FindObjectOfType<BomberController>();
            helper.InitialiseBuildable(_bomber);
			_bomber.StartConstruction();

            // Setup fighter
            _fighter = FindObjectOfType<FighterController>();
            helper.InitialiseBuildable(_fighter);
            _fighter.StartConstruction();

			// When completed, aircraft switches to patrol movement controller.
			// Hence wait a bit after completed before setting kamikaze
			// homing movement controller.
			//_aircraft.CompletedBuildable += (sender, e) => Invoke("Kamikaze", time: 0.1f);
			_aircraft.CompletedBuildable += (sender, e) => Invoke("Kamikaze", time: 1);
		}

        public void Kamikaze()
        {
            _aircraft.Kamikaze(_target);
            _bomber.Kamikaze(_target);
            _fighter.Kamikaze(_target);
        }
	}
}
