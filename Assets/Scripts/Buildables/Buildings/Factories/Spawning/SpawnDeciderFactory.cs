using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    public class SpawnDeciderFactory : ISpawnDeciderFactory
    {
        private readonly ITime _time;

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
                    new SpaceSpawnDecider(
                        factory,
                        spawnPositionFinder),
                    new CooldownSpawnDecider(
                        new UnitSpawnTimer(
                            factory,
                            _time)));
        }
    }
}