namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Deciders
{
    public interface IPvPBasicMover
    {
        bool IsMoving { get; }

        void StartMoving();
        void StopMoving();
    }
}
