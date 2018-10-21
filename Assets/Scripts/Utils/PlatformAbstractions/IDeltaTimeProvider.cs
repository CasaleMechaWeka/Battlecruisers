namespace BattleCruisers.Utils.PlatformAbstractions
{
    public interface IDeltaTimeProvider
    {
        float UnscaledDeltaTime { get; }
        float DeltaTime { get; }
    }
}
