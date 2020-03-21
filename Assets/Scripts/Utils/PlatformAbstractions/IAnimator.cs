namespace BattleCruisers.Utils.PlatformAbstractions
{
    public interface IAnimator
    {
        float Speed { get; set; }
        void Play(string stateName, int layer, float normalizedTime);
    }
}