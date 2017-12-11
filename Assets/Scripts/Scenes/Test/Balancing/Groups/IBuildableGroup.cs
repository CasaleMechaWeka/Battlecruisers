using System;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Scenes.Test.Balancing.Groups
{
    public interface IBuildableGroup
    {
        IPrefabKey BuildableKey { get; }
        int NumOfBuildables { get; }

        event EventHandler BuildablesDestroyed;

        void DestroyAllBuildables();
    }
}
