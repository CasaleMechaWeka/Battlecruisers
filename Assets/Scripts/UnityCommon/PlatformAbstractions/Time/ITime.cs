namespace UnityCommon.PlatformAbstractions.Time
{
    public interface ITime : IDeltaTimeProvider
    {
        float TimeScale { get; set; }
        float TimeSinceGameStartInS { get; }
        float UnscaledTimeSinceGameStartInS { get; }
        float UnscaledDeltaTime { get; }

        ITimeSinceGameStartProvider TimeSinceGameStartProvider { get; }
        ITimeSinceGameStartProvider RealTimeSinceGameStartProvider { get; }
    }
}