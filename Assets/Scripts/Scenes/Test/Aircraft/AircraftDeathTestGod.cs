using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
    public class AircraftDeathTestGod : MonoBehaviour
    {
        private AircraftController[] _aircraftList;

        void Start()
        {
            Helper helper = new Helper();
            _aircraftList = FindObjectsOfType<AircraftController>();

            foreach (AircraftController aircraft in _aircraftList)
            {
                helper.InitialiseUnit(aircraft);
                aircraft.StartConstruction();
            }

            Invoke("DestroyAircraft", 1);
        }

        private void DestroyAircraft()
        {
            foreach (AircraftController aircraft in _aircraftList)
            {
                aircraft.Destroy();
            }
        }
    }
}