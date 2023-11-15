namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.PositionValidators
{
    public interface IPvPTargetPositionValidatorFactory
    {
        IPvPTargetPositionValidator CreateDummyValidator();
        IPvPTargetPositionValidator CreateFacingMinRangeValidator(float minRangeInM);
    }
}
