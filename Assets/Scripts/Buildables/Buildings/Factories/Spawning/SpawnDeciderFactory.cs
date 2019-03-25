using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    public class SpawnDeciderFactory : ISpawnDeciderFactory
    {
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
                            new TimeBC())));
        }
    }
}