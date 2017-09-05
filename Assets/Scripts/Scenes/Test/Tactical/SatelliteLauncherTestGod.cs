using BattleCruisers.Buildables.Buildings.Offensive;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Offensive
{
    public class SatelliteLauncherTestGod : CameraToggleTestGod
    {
        public int numOfDrones;

        protected override void OnStart()
        {
            SatelliteLauncherController launcher = FindObjectOfType<SatelliteLauncherController>();

            Vector2 parentCruiserPosition = launcher.transform.position;
            Vector2 enemyCruiserPosition = new Vector2(launcher.transform.position.x + 30, launcher.transform.position.y);
            IAircraftProvider aircraftProvider = new AircraftProvider(parentCruiserPosition, enemyCruiserPosition);

            Helper helper = new Helper(numOfDrones);
            helper.InitialiseBuildable(launcher, aircraftProvider: aircraftProvider);
            launcher.StartConstruction();
        }
    }
}
