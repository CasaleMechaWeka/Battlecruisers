using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Aircraft.Satellites
{
    public class DeathstarDeathWhileFiringTestGod : TestGodBase
    {
        private SatelliteLauncherController _launcher;

        protected override void Start()
        {
            base.Start();

            Helper helper = new Helper(updaterProvider: _updaterProvider);

            // Satellite
            _launcher = FindObjectOfType<SatelliteLauncherController>();

            Vector2 parentCruiserPosition = _launcher.transform.position;
            Vector2 enemyCruiserPosition = new Vector2(_launcher.transform.position.x + 30, _launcher.transform.position.y);
            IAircraftProvider aircraftProvider = new AircraftProvider(parentCruiserPosition, enemyCruiserPosition, BCUtils.RandomGenerator.Instance);

            helper.InitialiseBuilding(_launcher, Faction.Blues, aircraftProvider: aircraftProvider);
            _launcher.StartConstruction();

            // Target
            AirFactory target = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(target, Faction.Reds);
            target.StartConstruction();
        }

        public void DestroyLaunchers()
        {
            _launcher.Destroy();
        }
    }
}
