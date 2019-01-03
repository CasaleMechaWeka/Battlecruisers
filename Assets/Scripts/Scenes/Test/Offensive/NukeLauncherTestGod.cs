using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Offensive;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;

namespace BattleCruisers.Scenes.Test.Offensive
{
    public class NukeLauncherTestGod : CameraToggleTestGod
	{
		protected override void OnStart()
		{
			Helper helper = new Helper();


			// Setup targets
            AirFactory basetarget = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(basetarget);

            DroneStation[] targets = FindObjectsOfType<DroneStation>();
            foreach (DroneStation target in targets)
            {
                helper.InitialiseBuilding(target);
            }


			// Setup nuke launcher
			ICruiser enemyCruiser = helper.CreateCruiser(basetarget.GameObject);
			IExactMatchTargetFilter targetFilter = Substitute.For<IExactMatchTargetFilter>();
			targetFilter.IsMatch(basetarget).Returns(true);
            ITargetFactoriesProvider targetFactories = helper.CreateTargetFactories(basetarget.GameObject, exactMatchTargetFilter: targetFilter);

			NukeLauncherController launcher = FindObjectOfType<NukeLauncherController>();
            helper.InitialiseBuilding(launcher, enemyCruiser: enemyCruiser, targetFactories: targetFactories);
			launcher.StartConstruction();
		}
	}
}
