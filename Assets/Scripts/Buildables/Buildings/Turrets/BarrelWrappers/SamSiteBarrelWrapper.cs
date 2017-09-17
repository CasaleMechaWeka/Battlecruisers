using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class SamSiteBarrelWrapper : DirectFireBarrelWrapper
	{
		protected override void InitialiseBarrelController(BarrelController barrelController)
        {
            SamSiteBarrelController samSiteBarrel = barrelController.Parse<SamSiteBarrelController>();

            IExactMatchTargetFilter targetFilter = _factoryProvider.TargetsFactory.CreateExactMatchTargetFilter();
			samSiteBarrel.Initialise(
                targetFilter, 
                CreateAngleCalculator(), 
                CreateRotationMovementController(),
                _factoryProvider.MovementControllerFactory,
                _factoryProvider.TargetPositionPredictorFactory);
        }
	}
}
