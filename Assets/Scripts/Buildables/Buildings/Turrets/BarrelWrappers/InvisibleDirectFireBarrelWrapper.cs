using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    /// <summary>
    /// Used by:
    /// + Tesla coil
    /// </summary>
    public class InvisibleDirectFireBarrelWrapper : DirectFireBarrelWrapper
    {
        protected override IRotationMovementController CreateRotationMovementController(IBarrelController barrel, IDeltaTimeProvider deltaTimeProvider)
        {
            return _factoryProvider.MovementControllerFactory.CreateDummyRotationMovementController();
        }

        protected override IAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.Turrets.AngleLimiterFactory.CreateDummyLimiter();
        }
    }
}
