namespace BattleCruisers.Movement.Deciders
{
    public interface IBasicMover
    {
        bool IsMoving { get; }

        void StartMoving();
        void StopMoving();
    }
}
