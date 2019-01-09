using BattleCruisers.Buildables.Units;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    public interface ISpawnPositionFinder
    {
        Vector3 FindSpawnPosition(IUnit unitToSpawn);
    }
}