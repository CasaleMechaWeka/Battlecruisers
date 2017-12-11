using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing.Groups
{
    public class UnitGroupController : BuildableGroupController
    {
        public override IBuildableGroup Initialise(
            IPrefabFactory prefabFactory,
            Helper helper,
            Faction faction,
            Direction facingDirection,
            Vector2 spawnPosition)
        {
            IPrefabKey buildableKey = StaticPrefabKeyHelper.GetPrefabKey(prefabKeyName);
            return new UnitGroup(buildableKey, numOfBuildables, prefabFactory, helper, faction, facingDirection, spawnPosition, SpacingMultiplier);
        }
    }
}
