namespace BattleCruisers.Effects
{
    public enum SmokeStrength
    {
        None, Weak, Normal, Strong
    }

    public interface ISmoke
    {
        SmokeStrength SmokeStrength { get; set; }
    }
}