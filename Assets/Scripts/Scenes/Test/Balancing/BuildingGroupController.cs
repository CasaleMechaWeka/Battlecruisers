using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class BuildingGroupController : MonoBehaviour 
    {
        public PrefabKeyName prefabKeyName;
        public int numOfBuildables;

        public IBuildableGroup Initialise(
            IPrefabFactory prefabFactory,
            Helper helper,
            Faction faction,
            Direction facingDirection,
            Vector2 spawnPosition)
        {
            IPrefabKey buildableKey = StaticPrefabKeyHelper.GetPrefabKey(prefabKeyName);
            return new BuildingGroup(buildableKey, numOfBuildables, prefabFactory, helper, faction, facingDirection, spawnPosition);
        }
    }
}
