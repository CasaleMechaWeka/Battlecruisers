using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.PositionValidators
{
    public interface IPvPTargetPositionValidatorFactory
    {
        ITargetPositionValidator CreateDummyValidator();
        ITargetPositionValidator CreateFacingMinRangeValidator(float minRangeInM);
    }
}
