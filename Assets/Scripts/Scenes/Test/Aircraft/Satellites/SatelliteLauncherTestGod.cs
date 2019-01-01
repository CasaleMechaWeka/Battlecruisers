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

        protected override void OnStart()
        {
            SatelliteLauncherController launcher = FindObjectOfType<SatelliteLauncherController>();

            Vector2 parentCruiserPosition = launcher.transform.position;
            Vector2 enemyCruiserPosition = new Vector2(launcher.transform.position.x + 30, launcher.transform.position.y);
            IAircraftProvider aircraftProvider = new AircraftProvider(parentCruiserPosition, enemyCruiserPosition, new BCUtils.RandomGenerator());

            float buildSpeedMultiplier = useFastBuildSpeed ? BCUtils.BuildSpeedMultipliers.VERY_FAST : BCUtils.BuildSpeedMultipliers.DEFAULT;
            Helper helper = new Helper(buildSpeedMultiplier: buildSpeedMultiplier);
            helper.InitialiseBuilding(launcher, aircraftProvider: aircraftProvider);
            launcher.StartConstruction();
        }
    }
}
