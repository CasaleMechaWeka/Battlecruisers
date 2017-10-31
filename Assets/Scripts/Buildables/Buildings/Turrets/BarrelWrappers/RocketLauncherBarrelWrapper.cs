using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class RocketLauncherBarrelWrapper : StaticBarrelWrapper
    {
        protected override float DesiredAngleInDegrees { get { return 60; } }

        protected override void InitialiseBarrelController(BarrelController barrel, ITargetFilter targetFilter, IAngleCalculator angleCalculator)
        {
            IBarrelControllerArgs args
                = new BarrelControllerArgs(
                    targetFilter,
                    CreateTargetPositionPredictor(),
                    angleCalculator,
                    CreateAccuracyAdjuster(angleCalculator, barrel),
                    CreateRotationMovementController(barrel),
                    _factoryProvider);

			RocketBarrelController rocketBarrel = barrel.Parse<RocketBarrelController>();
            rocketBarrel.Initialise(args, _enemyFaction);
        }
    }
}
