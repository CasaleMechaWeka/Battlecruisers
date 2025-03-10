using BattleCruisers.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time
{
    public class PvPRealTimeSinceGameStartProvider : ITimeSinceGameStartProvider
    {
        public float TimeSinceGameStartInS => PvPTimeBC.Instance.UnscaledTimeSinceGameStartInS;
    }
}