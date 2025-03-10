namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    public interface ISpawnDeciderFactory
    {
        IUnitSpawnPositionFinder CreateAircraftSpawnPositionFinder(IFactory factory);
        IUnitSpawnPositionFinder CreateNavalSpawnPositionFinder(IFactory factory);
        IUnitSpawnDecider CreateSpawnDecider(IFactory factory, IUnitSpawnPositionFinder spawnPositionFinder);
    }
}