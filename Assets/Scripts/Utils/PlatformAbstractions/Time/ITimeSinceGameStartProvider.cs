namespace BattleCruisers.Utils.PlatformAbstractions.Time
{
    public interface ITimeSinceGameStartProvider
    {
        float TimeSinceGameStartInS { get; }
    }
}