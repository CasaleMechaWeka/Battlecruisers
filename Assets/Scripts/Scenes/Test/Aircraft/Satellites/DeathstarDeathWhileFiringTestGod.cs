using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Aircraft.Satellites
{
    public class DeathstarDeathWhileFiringTestGod : TestGodBase
    {
        private SatelliteLauncherController _launcher;
        private AirFactory _target;

        protected override List<GameObject> GetGameObjects()
        {
            _launcher = FindObjectOfType<SatelliteLauncherController>();
            _target = FindObjectOfType<AirFactory>();

            return new List<GameObject>()
            {
                _launcher.GameObject,
                _target.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            // Satellite
            Vector2 parentCruiserPosition = _launcher.transform.position;
            Vector2 enemyCruiserPosition = new Vector2(_launcher.transform.position.x + 30, _launcher.transform.position.y);
            IAircraftProvider aircraftProvider = new AircraftProvider(parentCruiserPosition, enemyCruiserPosition, BCUtils.RandomGenerator.Instance);

            helper.InitialiseBuilding(_launcher, Faction.Blues, aircraftProvider: aircraftProvider);
            _launcher.StartConstruction();

            // Target
            helper.InitialiseBuilding(_target, Faction.Reds);
            _target.StartConstruction();
        }

        public void DestroyLaunchers()
        {
            _launcher.Destroy();
        }
    }
}
