namespace BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public class AttackablePositionFinderFactory
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