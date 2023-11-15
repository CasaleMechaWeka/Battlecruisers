namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public interface IPvPAttackablePositionFinderFactory
    {
        IPvPAttackablePositionFinder DummyPositionFinder { get; }

        IPvPAttackablePositionFinder CreateClosestPositionFinder();
    }
}