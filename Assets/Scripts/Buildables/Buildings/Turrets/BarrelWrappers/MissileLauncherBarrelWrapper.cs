using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class MissileLauncherBarrelWrapper : StaticBarrelWrapper
	{
        protected override float DesiredAngleInDegrees { get { return 90; } }

        protected override void InitialiseBarrelController(BarrelController barrel, ITargetFilter targetFilter, IAngleCalculator angleCalculator)
		{
            MissileBarrelController missileBarrel = barrel.Parse<MissileBarrelController>();

			missileBarrel.Initialise(
                targetFilter,
                angleCalculator,
                CreateRotationMovementController(barrel),
				_factoryProvider.MovementControllerFactory,
				_factoryProvider.TargetPositionPredictorFactory);
		}

        protected override IRotationMovementController CreateRotationMovementController(BarrelController barrel)
        {
            return _factoryProvider.MovementControllerFactory.CreateDummyRotationMovementController();
        }
	}
}
