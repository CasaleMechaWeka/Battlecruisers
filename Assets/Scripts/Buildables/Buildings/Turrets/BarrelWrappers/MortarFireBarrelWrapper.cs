using BattleCruisers.Movement.Predictors;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class MortarFireBarrelWrapper : GravityAffectedBarrelWrapper
	{
        protected override ITargetPositionPredictor CreateTargetPositionPredictor()
        {
            return _factoryProvider.TargetPositionPredictorFactory.CreateMortarPredictor();
        }

        protected override PositionValidators.ITargetPositionValidator CreatePositionValidator()
        {
            return _factoryProvider.Turrets.TargetPositionValidatorFactory.CreateFacingMinRangeValidator(_minRangeInM);
        }
	}
}
