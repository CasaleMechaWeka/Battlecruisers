using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Satellites
{
    public class SpySatelliteTestGod : TestGodBase
    {
        private SpySatelliteController _satellite;

        protected override List<GameObject> GetGameObjects()
        {
            _satellite = FindObjectOfType<SpySatelliteController>();
            return new List<GameObject>()
            {
                _satellite.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            Vector2 parentCruiserPosition = new Vector2(-35, 0);
            Vector2 enemyCruiserPosition = new Vector2(35, 0);
            IAircraftProvider aircraftProvider = new AircraftProvider(parentCruiserPosition, enemyCruiserPosition);

            helper.InitialiseUnit(_satellite, aircraftProvider: aircraftProvider);
            _satellite.StartConstruction();
        }
    }
}
