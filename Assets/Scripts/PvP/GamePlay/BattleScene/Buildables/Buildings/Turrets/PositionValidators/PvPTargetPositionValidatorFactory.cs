using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.PositionValidators
{
    public class PvPTargetPositionValidatorFactory : IPvPTargetPositionValidatorFactory
    {
        public ITargetPositionValidator CreateDummyValidator()
        {
            return new PvPDummyPositionValidator();
        }

        public ITargetPositionValidator CreateFacingMinRangeValidator(float minRangeInM)
        {
            return new PvPFacingMinRangePositionValidator(minRangeInM);
        }
    }
}
