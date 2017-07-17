using BattleCruisers.Buildables.Buildings.Offensive;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Offensive
{
	public class NukeLauncherTestGod : CameraToggleTestGod
	{
		protected override void OnStart()
		{
			NukeLauncherController launcher = GameObject.FindObjectOfType<NukeLauncherController>();

//			Vector2 parentCruiserPosition = launcher.transform.position;
//			Vector2 enemyCruiserPosition = new Vector2(launcher.transform.position.x + 30, launcher.transform.position.y);
//			IAircraftProvider aircraftProvider = new AircraftProvider(parentCruiserPosition, enemyCruiserPosition);

			Helper helper = new Helper();
			helper.InitialiseBuildable(launcher);
			launcher.StartConstruction();
		}
	}
}
