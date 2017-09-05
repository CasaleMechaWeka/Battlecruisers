using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
    public class SpySatelliteTestGod : MonoBehaviour
    {
        void Start()
        {
            Helper helper = new Helper();

            Vector2 parentCruiserPosition = new Vector2(-35, 0);
            Vector2 enemyCruiserPosition = new Vector2(35, 0);
            IAircraftProvider aircraftProvider = new AircraftProvider(parentCruiserPosition, enemyCruiserPosition);

            SpySatelliteController satellite = FindObjectOfType<SpySatelliteController>();
            helper.InitialiseBuildable(satellite, aircraftProvider: aircraftProvider);
            satellite.StartConstruction();
        }
    }
}
