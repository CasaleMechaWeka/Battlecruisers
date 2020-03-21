namespace BattleCruisers.Effects.Movement
{
    public interface IMovementEffect
    {
        void Show();
        void StartEffects();
        void StopEffects();
        void ResetAndHide();
    }
}