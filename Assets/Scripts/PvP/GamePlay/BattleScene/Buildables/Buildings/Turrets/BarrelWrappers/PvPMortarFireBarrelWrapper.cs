using BattleCruisers.Movement.Predictors;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class PvPMortarFireBarrelWrapper : PvPGravityAffectedBarrelWrapper
    {
        protected override ITargetPositionPredictor CreateTargetPositionPredictor()
        {
            return _factoryProvider.TargetPositionPredictorFactory.CreateMortarPredictor();
        }

        protected override PositionValidators.IPvPTargetPositionValidator CreatePositionValidator()
        {
            return _factoryProvider.Turrets.TargetPositionValidatorFactory.CreateFacingMinRangeValidator(_minRangeInM);
        }
    }
}
