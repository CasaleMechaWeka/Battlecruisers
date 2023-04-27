using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning
{
    public interface IPvPUnitSpawnPositionFinder
    {
        Vector3 FindSpawnPosition(IPvPUnit unitToSpawn);
    }
}