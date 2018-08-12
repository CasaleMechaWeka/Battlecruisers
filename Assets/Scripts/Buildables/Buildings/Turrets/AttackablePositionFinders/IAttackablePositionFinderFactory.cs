namespace BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public interface IAttackablePositionFinderFactory
    {
        IAttackablePositionFinder CreateClosestPositionFinder();
        IAttackablePositionFinder CreateDummyPositionFinder();
    }
}