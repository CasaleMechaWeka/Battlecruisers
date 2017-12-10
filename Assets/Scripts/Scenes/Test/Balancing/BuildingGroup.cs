using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Scenes.Test.Balancing.Spawners;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class BuildingGroup : BuildableGroup
    {
        public BuildingGroup(IPrefabKey buildableKey, int numOfBuildables, IPrefabFactory prefabFactory, Helper helper, Faction faction, Direction facingDirection, Vector2 spawnPosition) 
            : base(buildableKey, numOfBuildables, prefabFactory, helper, faction, facingDirection, spawnPosition)
        {
        }

        protected override IBuildableSpawner CreateSpawner(IPrefabFactory prefabFactory, Helper helper)
        {
            return new BuildingSpawner(prefabFactory, helper);
        }
    }
}
