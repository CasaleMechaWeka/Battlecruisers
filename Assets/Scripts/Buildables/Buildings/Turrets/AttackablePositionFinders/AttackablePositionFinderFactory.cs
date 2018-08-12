namespace BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public class AttackablePositionFinderFactory : IAttackablePositionFinderFactory
    {
        public IAttackablePositionFinder DummyPositionFinder { get; private set; }

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