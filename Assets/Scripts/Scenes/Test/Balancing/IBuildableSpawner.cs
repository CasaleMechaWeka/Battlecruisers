using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public interface IBuildableSpawner
    {
        IList<IBuildable> SpawnBuildables(
            IPrefabKey buildableKey, 
            int numOfBuildables, 
            Faction faction, 
            Direction facingDirection, 
            Vector2 spawnPosition,
            float spacingMultiplier = BuildableSpawner.DEFAULT_SPACING_MULTIPLIER);
    }
}
