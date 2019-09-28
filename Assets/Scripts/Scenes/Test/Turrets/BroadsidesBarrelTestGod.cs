using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;

namespace BattleCruisers.Scenes.Test
{
    public class BroadsidesBarrelTestGod : TestGodBase
	{
        protected override void Start()
        {
            base.Start();

            // Initialise target
            Helper helper = new Helper();
            Factory target = FindObjectOfType<Factory>();
            helper.InitialiseBuilding(target);


            // Initialise double barrel
            BarrelController doubleBarrel = FindObjectOfType<BarrelController>();
			doubleBarrel.StaticInitialise();
			doubleBarrel.Target = target;

            IBarrelControllerArgs barrelControllerArgs
                = helper.CreateBarrelControllerArgs(
                    doubleBarrel,
                    _updaterProvider.PerFrameUpdater,
                    targetFilter: new ExactMatchTargetFilter() { Target = target },
                    angleCalculator: new ArtilleryAngleCalculator(new AngleHelper(), doubleBarrel.ProjectileStats, new AngleConverter()));

            doubleBarrel.Initialise(barrelControllerArgs);
		}
	}
}
