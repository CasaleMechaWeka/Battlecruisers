namespace BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public interface IAttackablePositionFinderFactory
    {
        IAttackablePositionFinder DummyPositionFinder { get; }

        IAttackablePositionFinder CreateClosestPositionFinder();
    }
}