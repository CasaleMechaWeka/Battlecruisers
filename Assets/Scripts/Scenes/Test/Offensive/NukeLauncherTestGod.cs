using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Offensive;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Offensive
{
	public class NukeLauncherTestGod : CameraToggleTestGod
	{
		protected override void OnStart()
		{
			NukeLauncherController launcher = GameObject.FindObjectOfType<NukeLauncherController>();
			AirFactory target = GameObject.FindObjectOfType<AirFactory>();

			Helper helper = new Helper();

			ICruiser enemyCruiser = helper.CreateCruiser(target.GameObject);
			ITargetsFactory targetsFactory = helper.CreateTargetsFactory(target.GameObject);

			helper.InitialiseBuildable(launcher, enemyCruiser: enemyCruiser, targetsFactory: targetsFactory);
			launcher.StartConstruction();
		}
	}
}
