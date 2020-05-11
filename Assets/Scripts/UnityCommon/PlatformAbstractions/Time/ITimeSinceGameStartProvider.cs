namespace UnityCommon.PlatformAbstractions.Time
{
    public interface ITimeSinceGameStartProvider
    {
        float TimeSinceGameStartInS { get; }
    }
}