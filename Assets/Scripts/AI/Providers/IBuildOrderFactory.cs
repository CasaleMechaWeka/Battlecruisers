using System.Collections.Generic;
using BattleCruisers.AI.Providers.Strategies.Requests;
using BattleCruisers.Cruisers;

namespace BattleCruisers.AI.Providers
{
    public interface IBuildOrderFactory
    {
        IDynamicBuildOrder CreateOffensiveBuildOrder(IList<IOffensiveRequest> requests, int numOfPlatformSlots);
        IDynamicBuildOrder CreateAntiAirBuildOrder(int levelNum, ISlotWrapper slotWrapper);
        IDynamicBuildOrder CreateAntiNavalBuildOrder(int levelNum, ISlotWrapper slotWrapper);
	}
}
