namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading
{
    public class PvPDeferrerProvider : IPvPDeferrerProvider
    {
        public IPvPDeferrer Deferrer { get; }
        public IPvPDeferrer RealTimeDeferrer { get; }

        public PvPDeferrerProvider(IPvPDeferrer deferrer, IPvPDeferrer realTimeDeferrer)
        {
            PvPHelper.AssertIsNotNull(deferrer, realTimeDeferrer);

            Deferrer = deferrer;
            RealTimeDeferrer = realTimeDeferrer;
        }
    }
}