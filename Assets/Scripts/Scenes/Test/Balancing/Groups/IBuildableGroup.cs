using System;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Scenes.Test.Balancing.Groups
{
    public interface IBuildableGroup
    {
        IPrefabKey BuildableKey { get; }
        int NumOfBuildables { get; }
        ReadOnlyCollection<IBuildable> Buildables { get; }

        event EventHandler BuildablesDestroyed;

        void DestroyAllBuildables();
    }
}
