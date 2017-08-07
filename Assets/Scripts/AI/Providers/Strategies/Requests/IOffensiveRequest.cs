using BattleCruisers.AI.Providers.BuildingKey;

namespace BattleCruisers.AI.Providers.Strategies.Requests
{
    public interface IOffensiveRequest : IBasicOffensiveRequest
    {
        IBuildingKeyProvider BuildingKeyProvider { get; }
        int NumOfSlotsToUse { get; set; }
    }
}
