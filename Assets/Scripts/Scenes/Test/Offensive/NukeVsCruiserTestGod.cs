using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Offensive;
using BattleCruisers.Buildables.Buildings.Tactical.Shields;
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

            // Setup cruiser
            Cruiser cruiser = FindObjectOfType<Cruiser>();
            Assert.IsNotNull(cruiser);
            Helper.SetupCruiser(cruiser);

            // Setup shield
            Helper helper = new Helper(updaterProvider: _updaterProvider);
            ShieldGenerator shield = FindObjectOfType<ShieldGenerator>();
            Assert.IsNotNull(shield);
            helper.InitialiseBuilding(shield, Faction.Reds);
            shield.StartConstruction();

			// Setup nuke launcher
			IExactMatchTargetFilter targetFilter = Substitute.For<IExactMatchTargetFilter>();
			targetFilter.IsMatch(cruiser).Returns(true);
            ITargetFactories targetFactories = helper.CreateTargetFactories(cruiser.GameObject, exactMatchTargetFilter: targetFilter);

			NukeLauncherController launcher = FindObjectOfType<NukeLauncherController>();
            helper.InitialiseBuilding(launcher, Faction.Blues, enemyCruiser: cruiser, targetFactories: targetFactories);
			launcher.StartConstruction();
		}
	}
}
