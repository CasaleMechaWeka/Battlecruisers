using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Movement.Predictors;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class MortarFireBarrelWrapper : GravityAffectedBarrelWrapper
    {
        protected override ITargetPositionPredictor CreateTargetPositionPredictor()
        {
            return new MortarTargetPositionPredictor();
        }

        protected override PositionValidators.ITargetPositionValidator CreatePositionValidator()
        {
            return new FacingMinRangePositionValidator(_minRangeInM);
        }
    }
}
