namespace BattleCruisers.Cruisers.Slots.Feedback
{
    public enum BoostState
    {
        Off,    // No boost
        Single, // Single boost
        Double  // Double boost
    }

    public interface IBoostFeedback
    {
        BoostState State { get; set; }
    }
}