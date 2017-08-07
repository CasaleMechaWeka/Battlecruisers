using System.Collections.Generic;
using BattleCruisers.AI.Providers.Strategies;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers
{
    public interface IOffensiveBuildOrderProvider
    {
        IList<IPrefabKey> CreateBuildOrder(int numOfPlatformSlots, IList<IOffensiveRequest> requests);
    }
}
