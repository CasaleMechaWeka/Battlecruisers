using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public abstract class BuildableGroup : IBuildableGroup
    {
        private readonly IList<IBuildable> _aliveBuildables;

        public IPrefabKey BuildableKey { get; private set; }
        public int NumOfBuildables { get; private set; }

        public event EventHandler BuildablesDestroyed;

        protected BuildableGroup(IPrefabKey buildableKey, int numOfBuildables)
        {
            Assert.IsNotNull(buildableKey);
            Assert.IsTrue(numOfBuildables > 0);

            BuildableKey = buildableKey;
            NumOfBuildables = numOfBuildables;

            _aliveBuildables = CreateBuildables(BuildableKey, NumOfBuildables);

            foreach (IBuildable buildable in _aliveBuildables)
            {
                buildable.Destroyed += Buildable_Destroyed;
            }
        }

        protected abstract IList<IBuildable> CreateBuildables(IPrefabKey buildableKey, int numOfBuildables);

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
