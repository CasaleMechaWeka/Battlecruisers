using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
    public class AircraftDeathTestGod : MonoBehaviour
    {
        private AircraftController[] _aircraftList;

        async void Start()
        {
            _aircraftList = FindObjectsOfType<AircraftController>();
            Helper.SetActiveness(_aircraftList, false);

            Helper helper = await HelperFactory.CreateHelperAsync();

            foreach (AircraftController aircraft in _aircraftList)
            {
                aircraft.GameObject.SetActive(true);
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