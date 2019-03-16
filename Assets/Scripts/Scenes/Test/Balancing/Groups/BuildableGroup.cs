using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
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

        public IPrefabKey BuildableKey { get; }
        public int NumOfBuildables { get; }
        public ReadOnlyCollection<IBuildable> Buildables { get; }

        public event EventHandler BuildablesDestroyed;

        protected BuildableGroup(
            IPrefabKey buildableKey, 
            int numOfBuildables, 
            IPrefabFactory prefabFactory, 
            TestUtils.Helper helper,
            TestUtils.BuildableInitialisationArgs args,
            Vector2 spawnPosition,
            float spacingMultiplier)
        {
            Helper.AssertIsNotNull(buildableKey, prefabFactory, helper, args);
            Assert.IsTrue(numOfBuildables > 0);

            BuildableKey = buildableKey;
            NumOfBuildables = numOfBuildables;
			
            IBuildableSpawner spawner = CreateSpawner(prefabFactory, helper);
			
            _aliveBuildables = spawner.SpawnBuildables(BuildableKey, NumOfBuildables, args, spawnPosition, spacingMultiplier);

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

            if (_aliveBuildables.Count == 0)
            {
                BuildablesDestroyed?.Invoke(this, EventArgs.Empty);
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
