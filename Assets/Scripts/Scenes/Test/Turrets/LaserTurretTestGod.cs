using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;

namespace BattleCruisers.Scenes.Test.Turrets
{
    public class LaserTurretTestGod : TestGodBase
    {
        protected override void Start()
        {
            base.Start();

            Helper helper = new Helper();


            // Setup laser barrel
            LaserBarrelController laserBarrel = FindObjectOfType<LaserBarrelController>();
            laserBarrel.StaticInitialise();

            IBarrelControllerArgs barrelControllerArgs
                = helper.CreateBarrelControllerArgs(
                    laserBarrel,
                    _updaterProvider.PerFrameUpdater,
                    targetFilter: new DummyTargetFilter(isMatchResult: true),
                    rotationMovementController: new DummyRotationMovementController(isOnTarget: true));

            laserBarrel.Initialise(barrelControllerArgs);
            

            // Setup target
            AirFactory airFactory = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(airFactory);
            airFactory.Destroyed += (sender, e) => laserBarrel.Target = null;
			laserBarrel.Target = airFactory;
	    }
	}
}
