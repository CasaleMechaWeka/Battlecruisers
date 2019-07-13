namespace UnityCommon.PlatformAbstractions
{
    public interface ITime : IDeltaTimeProvider
    {
        float TimeScale { get; set; }
        float TimeSinceGameStartInS { get; }
    }
}