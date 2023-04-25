namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time
{
    public class PvPRealTimeSinceGameStartProvider : IPvPTimeSinceGameStartProvider
    {
        public float TimeSinceGameStartInS => PvPTimeBC.Instance.UnscaledTimeSinceGameStartInS;
    }
}