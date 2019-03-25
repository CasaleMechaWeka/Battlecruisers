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

        public IUnitSpawnDecider CreateAircraftSpawnDecider(IFactory factory)
        {
            return CreateSpawnDecider(factory, new AirFactorySpawnPositionFinder(factory));
        }

        public IUnitSpawnDecider CreateNavalSpawnDecider(IFactory factory)
        {
            return CreateSpawnDecider(factory, new NavalFactorySpawnPositionFinder(factory));
        }

        private IUnitSpawnDecider CreateSpawnDecider(IFactory factory, IUnitSpawnPositionFinder spawnPositionFinder)
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