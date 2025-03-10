using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing.Spawners
{
    public interface IBuildableSpawner
    {
        IList<IBuildable> SpawnBuildables(
            IPrefabKey buildableKey, 
            int numOfBuildables, 
            BuildableInitialisationArgs args,
            Vector2 spawnPosition,
            float spacingMultiplier = BuildableSpawner.DEFAULT_SPACING_MULTIPLIER);
    }
}
