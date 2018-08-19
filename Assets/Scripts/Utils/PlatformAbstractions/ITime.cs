namespace BattleCruisers.Utils.PlatformAbstractions
{
    public interface ITime
    {
        float TimeScale { get; set; }
        float TimeSinceGameStartInS { get; }
    }
}