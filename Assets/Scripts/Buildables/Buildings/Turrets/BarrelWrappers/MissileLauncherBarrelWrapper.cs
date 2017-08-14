using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class MissileLauncherBarrelWrapper : StaticBarrelWrapper
	{
        protected override float DesiredAngleInDegrees { get { return 90; } }

        protected override void InitialiseBarrelController()
		{
            MissileBarrelController barrelController = _barrelController as MissileBarrelController;
			Assert.IsNotNull(barrelController);

			barrelController.Initialise(
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
