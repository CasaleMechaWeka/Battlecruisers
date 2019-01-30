using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Scenes.Test.Balancing.Spawners;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing.Groups
{
    public abstract class BuildableGroupController : MonoBehaviour 
    {
        public PrefabKeyName prefabKeyName;
        public int numOfBuildables;
        public float spacingMultiplier;

        protected float SpacingMultiplier
        {
            get
            {
                return spacingMultiplier != default(float) ? spacingMultiplier : BuildableSpawner.DEFAULT_SPACING_MULTIPLIER;
            }
        }

        public IBuildableGroup Initialise(
            IPrefabFactory prefabFactory,
            Helper helper,
            BuildableInitialisationArgs args,
            Vector2 spawnPosition)
        {
            IPrefabKey buildableKey = StaticPrefabKeyHelper.GetPrefabKey<BuildingKey>(prefabKeyName);
            return CreateGroup(buildableKey, prefabFactory, helper, args, spawnPosition);
        }

        protected abstract IBuildableGroup CreateGroup(
            IPrefabKey buildableKey,
            IPrefabFactory prefabFactory, 
            Helper helper, 
            BuildableInitialisationArgs args, 
            Vector2 spawnPosition);
    }
}
