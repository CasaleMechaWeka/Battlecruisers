using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using BCUtils = BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Satellites
{
    public class SpySatelliteTestGod : MonoBehaviour
    {
        void Start()
        {
            Helper helper = new Helper();

            Vector2 parentCruiserPosition = new Vector2(-35, 0);
            Vector2 enemyCruiserPosition = new Vector2(35, 0);
            IAircraftProvider aircraftProvider = new AircraftProvider(parentCruiserPosition, enemyCruiserPosition, BCUtils.RandomGenerator.Instance);

            SpySatelliteController satellite = FindObjectOfType<SpySatelliteController>();
            helper.InitialiseUnit(satellite, aircraftProvider: aircraftProvider);
            satellite.StartConstruction();
        }
    }
}
