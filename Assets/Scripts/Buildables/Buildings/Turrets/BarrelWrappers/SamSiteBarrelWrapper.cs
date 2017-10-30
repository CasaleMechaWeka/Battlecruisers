using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class SamSiteBarrelWrapper : DirectFireBarrelWrapper
	{
        protected override void InitialiseBarrelController(BarrelController barrel, ITargetFilter targetFilter, IAngleCalculator angleCalculator)
        {
            IExactMatchTargetFilter exatMatchTargetFilter = _factoryProvider.TargetsFactory.CreateExactMatchTargetFilter();

            IBarrelControllerArgs args
                = new BarrelControllerArgs(
                    exatMatchTargetFilter,
                    CreateTargetPositionPredictor(),
                    angleCalculator,
                    CreateAccuracyAdjuster(),
                    CreateRotationMovementController(barrel),
                    _factoryProvider);

			SamSiteBarrelController samSiteBarrel = barrel.Parse<SamSiteBarrelController>();
            samSiteBarrel.Initialise(exatMatchTargetFilter, args);
        }
	}
}
