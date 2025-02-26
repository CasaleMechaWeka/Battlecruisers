using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public class PvPAttackablePositionFinderFactory : IPvPAttackablePositionFinderFactory
    {
        public IAttackablePositionFinder DummyPositionFinder { get; }

        public PvPAttackablePositionFinderFactory()
        {
            DummyPositionFinder = new PvPDummyPositionFinder();
        }

        public IAttackablePositionFinder CreateClosestPositionFinder()
        {
            return new PvPClosestPositionFinder();
        }
    }
}