namespace BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public interface IAttackablePositionFinderWrapper
    {
        IAttackablePositionFinder CreatePositionFinder();
    }
}