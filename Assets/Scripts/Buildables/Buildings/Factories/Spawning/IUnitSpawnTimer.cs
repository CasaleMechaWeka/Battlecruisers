namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    public interface IUnitSpawnTimer
    {
        /// <summary>
        /// The time in seconds since the factory last completed a unit,
        /// or an in progress unit was destroyed.
        /// </summary>
        float TimeSinceFactoryWasClearInS { get; }
    }
}