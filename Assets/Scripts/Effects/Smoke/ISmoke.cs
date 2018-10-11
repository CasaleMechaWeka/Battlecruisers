namespace BattleCruisers.Effects.Smoke
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