namespace UnityCommon.PlatformAbstractions
{
    public interface ITimeSinceGameStartProvider
    {
        float TimeSinceGameStartInS { get; }
    }
}