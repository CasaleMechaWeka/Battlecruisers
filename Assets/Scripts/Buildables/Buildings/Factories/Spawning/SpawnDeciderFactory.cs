using BattleCruisers.Utils;
using UnityCommon.PlatformAbstractions.Time;

namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    public class SpawnDeciderFactory : ISpawnDeciderFactory
    {
        private readonly ITime _time;

        public SpawnDeciderFactory()
        {
            _time = TimeBC.Instance;
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
                        Constants.POPULATION_LIMIT));
        }
    }
}