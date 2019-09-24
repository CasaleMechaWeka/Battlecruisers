using BattleCruisers.Buildables.Buildings.Offensive;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Offensive
{
    public class NukeVsCruiserTestGod : CameraToggleTestGod
	{
		protected override void Start()
        {
            base.Start();

            // Setup targets
            Cruiser cruiser = FindObjectOfType<Cruiser>();
            Assert.IsNotNull(cruiser);
            Helper.SetupCruiser(cruiser);

			// Setup nuke launcher
            Helper helper = new Helper(updaterProvider: _updaterProvider);
			IExactMatchTargetFilter targetFilter = Substitute.For<IExactMatchTargetFilter>();
			targetFilter.IsMatch(cruiser).Returns(true);
            ITargetFactories targetFactories = helper.CreateTargetFactories(cruiser.GameObject, exactMatchTargetFilter: targetFilter);

			NukeLauncherController launcher = FindObjectOfType<NukeLauncherController>();
            helper.InitialiseBuilding(launcher, enemyCruiser: cruiser, targetFactories: targetFactories);
			launcher.StartConstruction();
		}
	}
}
