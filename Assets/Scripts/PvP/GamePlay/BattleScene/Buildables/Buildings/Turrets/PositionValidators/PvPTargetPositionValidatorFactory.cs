namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.PositionValidators
{
    public class PvPTargetPositionValidatorFactory : IPvPTargetPositionValidatorFactory
    {
        public IPvPTargetPositionValidator CreateDummyValidator()
        {
            return new PvPDummyPositionValidator();
        }

        public IPvPTargetPositionValidator CreateFacingMinRangeValidator(float minRangeInM)
        {
            return new PvPFacingMinRangePositionValidator(minRangeInM);
        }
    }
}
