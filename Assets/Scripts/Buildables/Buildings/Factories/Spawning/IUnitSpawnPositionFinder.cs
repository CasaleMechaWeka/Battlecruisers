using BattleCruisers.Buildables.Units;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    public interface IUnitSpawnPositionFinder
    {
        Vector3 FindSpawnPosition(IUnit unitToSpawn);
    }
}