namespace UnityCommon.PlatformAbstractions
{
    public interface IDeltaTimeProvider
    {
        float UnscaledDeltaTime { get; }
        float DeltaTime { get; }
    }
}
