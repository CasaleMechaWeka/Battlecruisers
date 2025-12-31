using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Scenes.Test.Balancing.Spawners;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing.Groups
{
    public class BuildingGroup : BuildableGroup
    {
        public BuildingGroup(
            IPrefabKey buildableKey, 
            int numOfBuildables, 
            Helper helper, 
            BuildableInitialisationArgs args,
            Vector2 spawnPosition, 
            float spacingMultiplier) 
            : base(buildableKey, numOfBuildables, helper, args, spawnPosition, spacingMultiplier)
        {
        }

        protected override IBuildableSpawner CreateSpawner(Helper helper)
        {
            return new BuildingSpawner(helper);
        }
    }
}
