using System.Collections.Generic;
using BattleCruisers.AI.Providers.Strategies.Requests;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers
{
    public interface IOffensiveBuildOrderProvider
    {
        IList<IPrefabKey> CreateBuildOrder(int numOfPlatformSlots, IList<IOffensiveRequest> requests);
    }
}
