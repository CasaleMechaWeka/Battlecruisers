namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning
{
    public interface IPvPSpawnDeciderFactory
    {
        IPvPUnitSpawnPositionFinder CreateAircraftSpawnPositionFinder(IPvPFactory factory);
        IPvPUnitSpawnPositionFinder CreateNavalSpawnPositionFinder(IPvPFactory factory);
        IPvPUnitSpawnDecider CreateSpawnDecider(IPvPFactory factory, IPvPUnitSpawnPositionFinder spawnPositionFinder);
    }
}