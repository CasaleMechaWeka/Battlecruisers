namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public class PvPAttackablePositionFinderFactory : IPvPAttackablePositionFinderFactory
    {
        public IPvPAttackablePositionFinder DummyPositionFinder { get; }

        public PvPAttackablePositionFinderFactory()
        {
            DummyPositionFinder = new PvPDummyPositionFinder();
        }

        public IPvPAttackablePositionFinder CreateClosestPositionFinder()
        {
            return new PvPClosestPositionFinder();
        }
    }
}