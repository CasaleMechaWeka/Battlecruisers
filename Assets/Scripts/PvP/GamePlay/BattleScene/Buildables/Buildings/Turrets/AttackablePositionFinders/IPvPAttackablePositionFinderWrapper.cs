using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public interface IPvPAttackablePositionFinderWrapper
    {
        IAttackablePositionFinder CreatePositionFinder();
    }
}