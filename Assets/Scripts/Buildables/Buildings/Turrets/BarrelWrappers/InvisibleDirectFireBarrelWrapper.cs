using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Rotation;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class InvisibleDirectFireBarrelWrapper : DirectFireBarrelWrapper
    {
        protected override IRotationMovementController CreateRotationMovementController(BarrelController barrel)
        {
            return _factoryProvider.MovementControllerFactory.CreateDummyRotationMovementController();
        }

        protected override IAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.AngleLimiterFactory.CreateDummyLimiter();
        }
    }
}
