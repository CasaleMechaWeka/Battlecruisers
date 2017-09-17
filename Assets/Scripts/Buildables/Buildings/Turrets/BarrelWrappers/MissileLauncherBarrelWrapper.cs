using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class MissileLauncherBarrelWrapper : StaticBarrelWrapper
	{
        protected override float DesiredAngleInDegrees { get { return 90; } }

        protected override void InitialiseBarrelController(BarrelController barrelController)
		{
            MissileBarrelController missileBarrel = barrelController.Parse<MissileBarrelController>();

			missileBarrel.Initialise(
                CreateTargetFilter(),
				CreateAngleCalculator(),
				CreateRotationMovementController(),
				_factoryProvider.MovementControllerFactory,
				_factoryProvider.TargetPositionPredictorFactory);
		}

        protected override Movement.Rotation.IRotationMovementController CreateRotationMovementController()
        {
            return _factoryProvider.MovementControllerFactory.CreateDummyRotationMovementController();
        }
	}
}
