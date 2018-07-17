using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Rotation;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    /// <summary>
    /// Used by:
    /// + Tesla coil
    /// </summary>
    public class InvisibleDirectFireBarrelWrapper : DirectFireBarrelWrapper
    {
        protected override IRotationMovementController CreateRotationMovementController(IBarrelController barrel)
        {
            return _factoryProvider.MovementControllerFactory.CreateDummyRotationMovementController();
        }

        protected override IAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.AngleLimiterFactory.CreateDummyLimiter();
        }
    }
}
