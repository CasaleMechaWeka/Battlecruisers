using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Scenes.Test.Balancing.Spawners;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Balancing.Groups
{
    public abstract class BuildableGroupController : MonoBehaviour
    {
        public BCUtils.PrefabKeyName prefabKeyName;
        public int numOfBuildables;
        public float spacingMultiplier;

        protected float SpacingMultiplier
        {
            get
            {
                return spacingMultiplier != default ? spacingMultiplier : BuildableSpawner.DEFAULT_SPACING_MULTIPLIER;
            }
        }

        public IBuildableGroup Initialise(
            Helper helper,
            BuildableInitialisationArgs args,
            Vector2 spawnPosition)
        {
            IPrefabKey buildableKey = GetBuildableKey(prefabKeyName);
            return CreateGroup(buildableKey, helper, args, spawnPosition);
        }

        protected abstract IPrefabKey GetBuildableKey(BCUtils.PrefabKeyName prefabKeyName);

        protected abstract IBuildableGroup CreateGroup(
            IPrefabKey buildableKey,
            Helper helper, 
            BuildableInitialisationArgs args, 
            Vector2 spawnPosition);
    }
}
