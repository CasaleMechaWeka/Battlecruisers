using BattleCruisers.AI.Providers.BuildingKey;

namespace BattleCruisers.AI.Providers.Strategies
{
    public enum OffensiveType
    {
        Air, Naval, Buildings, Ultras
    }

    public enum OffensiveFocus
    {
        Low, High
    }

    public interface IOffensiveRequest
    {
        OffensiveType Type { get; }
        OffensiveFocus Focus { get; }
        IBuildingKeyProvider BuildingKeyProvider { get; }
        int NumOfSlotsToUse { get; set; }
    }
}
