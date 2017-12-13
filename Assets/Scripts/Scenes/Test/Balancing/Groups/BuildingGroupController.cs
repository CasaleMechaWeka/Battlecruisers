using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing.Groups
{
    public class BuildingGroupController : BuildableGroupController
    {
        protected override IBuildableGroup CreateGroup(
            IPrefabKey buildableKey,
            IPrefabFactory prefabFactory,
            Helper helper,
            BuildableInitialisationArgs args,
            Vector2 spawnPosition)
        {
            return new BuildingGroup(buildableKey, numOfBuildables, prefabFactory, helper, args, spawnPosition, SpacingMultiplier);
        }
    }
}
