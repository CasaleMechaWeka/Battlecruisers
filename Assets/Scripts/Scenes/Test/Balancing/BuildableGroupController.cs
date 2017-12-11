using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Fetchers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public abstract class BuildableGroupController : MonoBehaviour 
    {
        public PrefabKeyName prefabKeyName;
        public int numOfBuildables;

        public abstract IBuildableGroup Initialise(
            IPrefabFactory prefabFactory,
            Helper helper,
            Faction faction,
            Direction facingDirection,
            Vector2 spawnPosition);
    }
}
