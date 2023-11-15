using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Deciders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships
{
    public interface IPvPShip : IPvPUnit, IPvPBasicMover
    {
        float OptimalArmamentRangeInM { get; }

        void DisableMovement();
    }
}
