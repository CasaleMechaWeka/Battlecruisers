using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Movement.Predictors;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class PvPMortarFireBarrelWrapper : PvPGravityAffectedBarrelWrapper
    {
        protected override ITargetPositionPredictor CreateTargetPositionPredictor()
        {
            return new MortarTargetPositionPredictor();
        }

        protected override ITargetPositionValidator CreatePositionValidator()
        {
            return new FacingMinRangePositionValidator(_minRangeInM);
        }
    }
}
