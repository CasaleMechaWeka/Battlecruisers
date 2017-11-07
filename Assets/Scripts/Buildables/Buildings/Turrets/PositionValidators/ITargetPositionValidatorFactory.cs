namespace BattleCruisers.Buildables.Buildings.Turrets.PositionValidators
{
    public interface ITargetPositionValidatorFactory
    {
        ITargetPositionValidator CreateDummyValidator();
        ITargetPositionValidator CreateFacingMinRangeValidator(float minRangeInM);
    }
}
