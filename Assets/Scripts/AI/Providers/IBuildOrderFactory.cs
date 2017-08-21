using System.Collections.Generic;
using BattleCruisers.AI.Providers.Strategies.Requests;

namespace BattleCruisers.AI.Providers
{
    public interface IBuildOrderFactory
    {
        IDynamicBuildOrder CreateOffensiveBuildOrder(IList<IOffensiveRequest> requests, int numOfPlatformSlots);
    }
}
