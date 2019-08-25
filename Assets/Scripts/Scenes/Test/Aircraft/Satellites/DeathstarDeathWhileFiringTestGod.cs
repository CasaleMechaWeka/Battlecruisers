using BattleCruisers.Buildables.Buildings;
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

            _launcher = FindObjectOfType<SatelliteLauncherController>();

            Vector2 parentCruiserPosition = _launcher.transform.position;
            Vector2 enemyCruiserPosition = new Vector2(_launcher.transform.position.x + 30, _launcher.transform.position.y);
            IAircraftProvider aircraftProvider = new AircraftProvider(parentCruiserPosition, enemyCruiserPosition, new BCUtils.RandomGenerator());

            helper.InitialiseBuilding(_launcher, aircraftProvider: aircraftProvider);
            _launcher.StartConstruction();
        }

        public void DestroyLaunchers()
        {
            _launcher.Destroy();
        }
    }
}
