namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies.Requests
{
    public enum PvPOffensiveType
    {
        Air, Naval, Buildings, Ultras
    }

    public enum PvPOffensiveFocus
    {
        Low, High
    }

    public interface IPvPOffensiveRequest
    {
        PvPOffensiveType Type { get; }
        PvPOffensiveFocus Focus { get; }
        int NumOfSlotsToUse { get; set; }
    }
}
