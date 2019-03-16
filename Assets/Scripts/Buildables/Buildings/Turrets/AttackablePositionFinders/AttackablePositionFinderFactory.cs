namespace BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public class AttackablePositionFinderFactory : IAttackablePositionFinderFactory
    {
        public IAttackablePositionFinder DummyPositionFinder { get; }

        public AttackablePositionFinderFactory()
        {
            DummyPositionFinder = new DummyPositionFinder();
        }

        public IAttackablePositionFinder CreateClosestPositionFinder()
        {
            return new ClosestPositionFinder();
        }
    }
}