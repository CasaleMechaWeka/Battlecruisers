namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time
{
    public class PvPTimeSinceGameStartProvider : IPvPTimeSinceGameStartProvider
    {
        public float TimeSinceGameStartInS => PvPTimeBC.Instance.TimeSinceGameStartInS;
    }
}