using UnityCommon.PlatformAbstractions;

namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    public class SpawnDeciderFactory : ISpawnDeciderFactory
    {
        private readonly ITime _time;

        // FELIX  Reduce to 30?
        private const int POPULATION_LIMIT = 50;

        public SpawnDeciderFactory()
        {
            _time = new TimeBC();
        }

        public IUnitSpawnPositionFinder CreateAircraftSpawnPositionFinder(IFactory factory)
        {
            return new AirFactorySpawnPositionFinder(factory);
        }

        public IUnitSpawnPositionFinder CreateNavalSpawnPositionFinder(IFactory factory)
        {
            return new NavalFactorySpawnPositionFinder(factory);
        }

        public IUnitSpawnDecider CreateSpawnDecider(IFactory factory, IUnitSpawnPositionFinder spawnPositionFinder)
        {
            return
                new CompositeSpawnDecider(
                    new CooldownSpawnDecider(
                        new UnitSpawnTimer(
                            factory,
                            _time)),
                    new SpaceSpawnDecider(
                        factory,
                        spawnPositionFinder),
                    new PopulationLimitSpawnDecider(
                        factory.ParentCruiser.UnitMonitor,
                        POPULATION_LIMIT));
        }
    }
}