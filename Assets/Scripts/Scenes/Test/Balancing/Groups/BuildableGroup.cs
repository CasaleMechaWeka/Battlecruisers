using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Scenes.Test.Balancing.Spawners;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing.Groups
{
    public abstract class BuildableGroup : IBuildableGroup
    {
        private readonly IList<IBuildable> _aliveBuildables;

        public IPrefabKey BuildableKey { get; private set; }
        public int NumOfBuildables { get; private set; }
        public ReadOnlyCollection<IBuildable> Buildables { get; private set; }

        public event EventHandler BuildablesDestroyed;

        protected BuildableGroup(
            IPrefabKey buildableKey, 
            int numOfBuildables, 
            IPrefabFactory prefabFactory, 
            TestUtils.Helper helper,
            Faction faction,
            Direction facingDirection,
            Vector2 spawnPosition,
            float spacingMultiplier)
        {
            Helper.AssertIsNotNull(buildableKey, prefabFactory, helper);
            Assert.IsTrue(numOfBuildables > 0);

            BuildableKey = buildableKey;
            NumOfBuildables = numOfBuildables;
			
            IBuildableSpawner spawner = CreateSpawner(prefabFactory, helper);
			
            _aliveBuildables = spawner.SpawnBuildables(BuildableKey, NumOfBuildables, faction, facingDirection, spawnPosition, spacingMultiplier);

			foreach (IBuildable buildable in _aliveBuildables)
			{
				buildable.Destroyed += Buildable_Destroyed;
			}

            Buildables = new ReadOnlyCollection<IBuildable>(_aliveBuildables);
        }

        protected abstract IBuildableSpawner CreateSpawner(IPrefabFactory prefabFactory, TestUtils.Helper helper);

        private void Buildable_Destroyed(object sender, DestroyedEventArgs e)
        {
            IBuildable destroyedBuildable = e.DestroyedTarget.Parse<IBuildable>();

            Assert.IsTrue(_aliveBuildables.Contains(destroyedBuildable));
            _aliveBuildables.Remove(destroyedBuildable);

            if (_aliveBuildables.Count == 0 && BuildablesDestroyed != null)
            {
                BuildablesDestroyed.Invoke(this, EventArgs.Empty);
            }
        }

        public void DestroyAllBuildables()
        {
            IList<IBuildable> aliveBuildablesCopy = _aliveBuildables.ToList();

            foreach (IBuildable buildable in aliveBuildablesCopy)
            {
                buildable.Destroy();
            }
        }
    }
}
