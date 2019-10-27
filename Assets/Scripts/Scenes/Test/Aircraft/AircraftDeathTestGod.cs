using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
    public class AircraftDeathTestGod : TestGodBase
    {
        private AircraftController[] _aircraftList;

        protected override List<GameObject> GetGameObjects()
        {
            _aircraftList = FindObjectsOfType<AircraftController>();
            return _aircraftList.Select(aircraft => aircraft.GameObject).ToList();
        }

        protected override void Setup(Helper helper)
        {
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