namespace BattleCruisers.Cruisers
{
    public interface ICruiserStats
    {
        float Health { get; }
        int NumOfDrones { get; }
        int NumOfPlatformSlots { get; }
        int NumOfDeckSlots { get; }
        int NumOfUtilitySlots { get; }
        int NumOfMastSlots { get; }
    }
}
