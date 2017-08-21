using BattleCruisers.AI.Providers.BuildingKey;

namespace BattleCruisers.AI.Providers.Strategies.Requests
{
    public interface IOffensiveRequest : IBasicOffensiveRequest
    {
        // FELIX  Remove
        IBuildingKeyProvider BuildingKeyProvider { get; }

        int NumOfSlotsToUse { get; set; }
    }
}
