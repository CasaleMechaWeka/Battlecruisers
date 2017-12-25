using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class RocketLauncherBarrelWrapper : StaticBarrelWrapper
    {
        protected override float DesiredAngleInDegrees { get { return 60; } }

        protected override void InitialiseBarrelController(BarrelController barrel, ITarget parent, ITargetFilter targetFilter, IAngleCalculator angleCalculator)
        {
            IBarrelControllerArgs args
                = new BarrelControllerArgs(
                    targetFilter,
                    CreateTargetPositionPredictor(),
                    angleCalculator,
                    CreateAccuracyAdjuster(angleCalculator, barrel),
                    CreateRotationMovementController(barrel),
                    CreatePositionValidator(),
                    CreateAngleLimiter(),
                    _factoryProvider,
                    parent);

            Faction ownFaction = Helper.GetOppositeFaction(_enemyFaction);
			RocketBarrelController rocketBarrel = barrel.Parse<RocketBarrelController>();
            rocketBarrel.Initialise(args, ownFaction);
        }
    }
}
