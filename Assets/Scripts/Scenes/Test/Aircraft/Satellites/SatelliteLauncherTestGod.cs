using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Offensive;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Aircraft.Satellites
{
    public class SatelliteLauncherTestGod : CameraToggleTestGod
    {
        public bool useFastBuildSpeed;

        private SatelliteLauncherController[] _launchers;

		protected override void Start()
        {
            base.Start();

            float buildSpeedMultiplier = useFastBuildSpeed ? BCUtils.BuildSpeedMultipliers.FAST : BCUtils.BuildSpeedMultipliers.DEFAULT;
            Helper helper = new Helper(buildSpeedMultiplier: buildSpeedMultiplier, updaterProvider: _updaterProvider);

            _launchers = FindObjectsOfType<SatelliteLauncherController>();

            foreach (SatelliteLauncherController launcher in _launchers)
            {
                Vector2 parentCruiserPosition = launcher.transform.position;
                Vector2 enemyCruiserPosition = new Vector2(launcher.transform.position.x + 30, launcher.transform.position.y);
                IAircraftProvider aircraftProvider = new AircraftProvider(parentCruiserPosition, enemyCruiserPosition, BCUtils.RandomGenerator.Instance);

                helper.InitialiseBuilding(launcher, aircraftProvider: aircraftProvider);
                launcher.StartConstruction();
            }
        }

        public void DestroyLaunchers()
        {
            foreach (SatelliteLauncherController launcher in _launchers)
            {
                launcher.Destroy();
            }
        }
    }
}
