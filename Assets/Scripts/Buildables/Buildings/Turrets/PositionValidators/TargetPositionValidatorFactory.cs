namespace BattleCruisers.Buildables.Buildings.Turrets.PositionValidators
{
    public class TargetPositionValidatorFactory : ITargetPositionValidatorFactory
    {
        public ITargetPositionValidator CreateDummyValidator()
        {
            return new DummyPositionValidator();
        }

        public ITargetPositionValidator CreateFacingMinRangeValidator(float minRangeInM)
        {
            return new FacingMinRangePositionValidator(minRangeInM);
        }
    }
}
