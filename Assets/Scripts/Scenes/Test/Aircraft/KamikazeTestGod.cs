using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
    public class KamikazeTestGod : MonoBehaviour
	{
        private IFactory _target;
        private TestAircraftController _aircraft;

		void Start()
		{
			Helper helper = new Helper();
			
            // Setup target
			_target = FindObjectOfType<Factory>();
			helper.InitialiseBuildable(_target);
			_target.StartConstruction();

            // Setup aircraft
            _aircraft = FindObjectOfType<TestAircraftController>();
            helper.InitialiseBuildable(_aircraft);
            _aircraft.StartConstruction();

            // When completed, aircraft switches to patrol movement controller.
            // Hence wait a bit after completed before setting kamikaze
            // homing movement controller.
            _aircraft.CompletedBuildable += (sender, e) => Invoke("Kamikaze", time: 0.1f);
		}

        public void Kamikaze()
        {
            _aircraft.Kamikaze(_target);
        }
	}
}
