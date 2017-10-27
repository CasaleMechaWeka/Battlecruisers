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
            SamSiteBarrelController samSiteBarrel = barrel.Parse<SamSiteBarrelController>();

            IExactMatchTargetFilter exatMatchTargetFilter = _factoryProvider.TargetsFactory.CreateExactMatchTargetFilter();

            samSiteBarrel.Initialise(
                exatMatchTargetFilter,
                angleCalculator,
                CreateRotationMovementController(barrel),
                _factoryProvider);
        }
	}
}
