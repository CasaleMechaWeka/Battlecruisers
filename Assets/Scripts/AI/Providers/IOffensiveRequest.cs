namespace BattleCruisers.AI.Providers
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
