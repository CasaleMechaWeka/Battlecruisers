using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Balancing.Groups
{
    public class BuildingGroupController : BuildableGroupController
    {
        protected override IPrefabKey GetBuildableKey(BCUtils.PrefabKeyName prefabKeyName)
        {
            return BCUtils.StaticPrefabKeyHelper.GetPrefabKey<BuildingKey>(prefabKeyName);
        }

        protected override IBuildableGroup CreateGroup(
            IPrefabKey buildableKey,
            Helper helper,
            BuildableInitialisationArgs args,
            Vector2 spawnPosition)
        {
            return new BuildingGroup(buildableKey, numOfBuildables, helper, args, spawnPosition, SpacingMultiplier);
        }
    }
}
