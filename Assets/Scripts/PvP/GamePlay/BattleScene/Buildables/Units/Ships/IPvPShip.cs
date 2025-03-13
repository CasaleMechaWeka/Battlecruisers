using BattleCruisers.Movement.Deciders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships
{
    public interface IPvPShip : IPvPUnit, IBasicMover
    {
        float OptimalArmamentRangeInM { get; }
        bool KeepDistanceFromEnemyCruiser { get; }

        void DisableMovement();
    }
}
