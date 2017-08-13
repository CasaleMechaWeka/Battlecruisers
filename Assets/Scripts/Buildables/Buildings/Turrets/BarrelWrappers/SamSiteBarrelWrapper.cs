using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class SamSiteBarrelWrapper : DirectFireBarrelWrapper
	{
		protected override void InitialiseBarrelController()
        {
			SamSiteBarrelController barrelController = _barrelController as SamSiteBarrelController;
			Assert.IsNotNull(barrelController);

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
