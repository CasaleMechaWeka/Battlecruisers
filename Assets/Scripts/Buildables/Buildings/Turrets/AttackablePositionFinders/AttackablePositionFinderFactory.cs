namespace BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public class AttackablePositionFinderFactory : IAttackablePositionFinderFactory
    {
        public IAttackablePositionFinder CreateClosestPositionFinder()
        {
            return new ClosestPositionFinder();
        }

        public IAttackablePositionFinder CreateDummyPositionFinder()
        {
            return new DummyPositionFinder();
        }
    }
}