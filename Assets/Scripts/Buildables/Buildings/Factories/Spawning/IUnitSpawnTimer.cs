namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    public interface IUnitSpawnTimer
    {
        float TimeSinceLastUnitStartedInS { get; }
    }
}