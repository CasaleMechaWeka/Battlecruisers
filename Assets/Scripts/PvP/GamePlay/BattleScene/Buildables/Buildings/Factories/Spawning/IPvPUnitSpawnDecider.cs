using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning
{
    public interface IPvPUnitSpawnDecider
    {
        bool CanSpawnUnit(IPvPUnit unitToSpawn);
    }
}