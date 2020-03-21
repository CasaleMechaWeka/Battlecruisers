namespace BattleCruisers.Effects.Movement
{
    // FELIX  Rename to remove plural
    public interface IMovementEffects
    {
        void Show();
        void StartEffects();
        void StopEffects();
        void ResetAndHide();
    }
}