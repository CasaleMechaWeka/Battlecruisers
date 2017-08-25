using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class SamSiteBarrelWrapper : DirectFireBarrelWrapper
	{
		protected override void InitialiseBarrelController()
        {
			SamSiteBarrelController barrelController = _barrelController.Parse<SamSiteBarrelController>();

            IExactMatchTargetFilter targetFilter = _factoryProvider.TargetsFactory.CreateExactMatchTargetFilter();
			barrelController.Initialise(
                targetFilter, 
                CreateAngleCalculator(), 
                CreateRotationMovementController(),
                _factoryProvider.MovementControllerFactory,
                _factoryProvider.TargetPositionPredictorFactory);
        }
	}
}
