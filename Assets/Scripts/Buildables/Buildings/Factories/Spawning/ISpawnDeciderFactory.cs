namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    public interface ISpawnDeciderFactory
    {
        IUnitSpawnDecider CreateAircraftSpawnDecider(IFactory factory);
        IUnitSpawnDecider CreateNavalSpawnDecider(IFactory factory);
    }
}