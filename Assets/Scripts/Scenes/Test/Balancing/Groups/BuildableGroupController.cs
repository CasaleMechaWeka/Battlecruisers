using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Fetchers;
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

        public abstract IBuildableGroup Initialise(
            IPrefabFactory prefabFactory,
            Helper helper,
            Faction faction,
            Direction facingDirection,
            Vector2 spawnPosition);
    }
}
