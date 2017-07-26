namespace BattleCruisers.Buildables.Buildings.Turrets.Stats.States
{
    public interface IBurstFireState
    {
        float DurationInS { get; }
        IBurstFireState NextState { get; }
        bool IsInBurst { get; }
    }
}
