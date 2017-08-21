using BattleCruisers.AI.Providers.Strategies.Requests;

namespace BattleCruisers.AI.Providers
{
    public interface IBuildOrderFactory
    {
        IDynamicBuildOrder CreateBuildOrder(IOffensiveRequest request);
    }
}
