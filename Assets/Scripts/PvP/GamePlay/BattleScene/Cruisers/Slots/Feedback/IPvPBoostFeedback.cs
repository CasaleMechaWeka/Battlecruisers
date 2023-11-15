namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.Feedback
{
    public enum PvPBoostState
    {
        Off,    // No boost
        Single, // Single boost
        Double  // Double boost
    }

    public interface IPvPBoostFeedback
    {
        PvPBoostState State { get; set; }
    }
}