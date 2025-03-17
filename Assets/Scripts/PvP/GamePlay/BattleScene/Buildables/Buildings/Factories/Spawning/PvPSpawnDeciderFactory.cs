using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning
{
    public class PvPSpawnDeciderFactory : IPvPSpawnDeciderFactory
    {
        private readonly ITime _time;

        public PvPSpawnDeciderFactory()
        {
            _time = TimeBC.Instance;
        }

        public IPvPUnitSpawnPositionFinder CreateAircraftSpawnPositionFinder(IPvPFactory factory)
        {
            return new PvPAirFactorySpawnPositionFinder(factory);
        }

        public IPvPUnitSpawnPositionFinder CreateNavalSpawnPositionFinder(IPvPFactory factory)
        {
            return new PvPNavalFactorySpawnPositionFinder(factory);
        }

        public IPvPUnitSpawnDecider CreateSpawnDecider(IPvPFactory factory, IPvPUnitSpawnPositionFinder spawnPositionFinder)
        {
            return
                new PvPCompositeSpawnDecider(
                    new PvPCooldownSpawnDecider(
                        new PvPUnitSpawnTimer(
                            factory,
                            _time)),
                    new PvPSpaceSpawnDecider(
                        factory,
                        spawnPositionFinder),
                    new PvPPopulationLimitSpawnDecider(
                        factory.ParentCruiser.UnitMonitor,
                        Constants.POPULATION_LIMIT));
        }
    }
}