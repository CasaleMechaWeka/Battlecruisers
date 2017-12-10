using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils;
using UnityEngine.Assertions;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public abstract class BuildableGroup : IBuildableGroup
    {
        private readonly IPrefabFactory _prefabFactory;
        private readonly TestUtils.Helper _helper;
        private IList<IBuildable> _aliveBuildables;

        public IPrefabKey BuildableKey { get; private set; }
        public int NumOfBuildables { get; private set; }

        public event EventHandler BuildablesDestroyed;

        protected BuildableGroup(IPrefabKey buildableKey, int numOfBuildables, IPrefabFactory prefabFactory, TestUtils.Helper helper)
        {
            Helper.AssertIsNotNull(buildableKey, prefabFactory, helper);
            Assert.IsTrue(numOfBuildables > 0);

            BuildableKey = buildableKey;
            NumOfBuildables = numOfBuildables;
            _prefabFactory = prefabFactory;
            _helper = helper;
        }

        public void SpawnBuildables()
        {
			_aliveBuildables = CreateBuildables();
			
			foreach (IBuildable buildable in _aliveBuildables)
			{
				buildable.Destroyed += Buildable_Destroyed;
			}
        }

        protected abstract IList<IBuildable> CreateBuildables();

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
