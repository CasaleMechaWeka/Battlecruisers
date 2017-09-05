using BattleCruisers.Buildables.Buildings.Offensive;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Offensive
{
    // FELIX  Avoid duplicate code with deathstar launcher test
	public class SpySatelliteLauncherTestGod : CameraToggleTestGod
	{
		protected override void OnStart()
		{
            SpySatelliteLauncherController launcher = FindObjectOfType<SpySatelliteLauncherController>();

			Vector2 parentCruiserPosition = launcher.transform.position;
			Vector2 enemyCruiserPosition = new Vector2(launcher.transform.position.x + 30, launcher.transform.position.y);
			IAircraftProvider aircraftProvider = new AircraftProvider(parentCruiserPosition, enemyCruiserPosition);

            Helper helper = new Helper(numOfDrones: 1);
			helper.InitialiseBuildable(launcher, aircraftProvider: aircraftProvider);
			launcher.StartConstruction();
		}
	}
}
