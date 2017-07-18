using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Offensive;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Offensive
{
	public class NukeLauncherTestGod : CameraToggleTestGod
	{
		protected override void OnStart()
		{
			Helper helper = new Helper();


			// Setup target
			AirFactory target = GameObject.FindObjectOfType<AirFactory>();
			helper.InitialiseBuildable(target);


			// Setup nuke launcher
			ICruiser enemyCruiser = helper.CreateCruiser(target.GameObject);
			IExactMatchTargetFilter targetFilter = Substitute.For<IExactMatchTargetFilter>();
			targetFilter.IsMatch(null).ReturnsForAnyArgs(true);
			ITargetsFactory targetsFactory = helper.CreateTargetsFactory(target.GameObject, exactMatchTargetFilter: targetFilter);

			NukeLauncherController launcher = GameObject.FindObjectOfType<NukeLauncherController>();
			helper.InitialiseBuildable(launcher, enemyCruiser: enemyCruiser, targetsFactory: targetsFactory);
			launcher.StartConstruction();
		}
	}
}
