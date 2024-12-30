using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading
{
    public class PvPDeferrerProvider : IPvPDeferrerProvider
    {
        public IDeferrer Deferrer { get; }
        public IDeferrer RealTimeDeferrer { get; }

        public PvPDeferrerProvider(IDeferrer deferrer, IDeferrer realTimeDeferrer)
        {
            PvPHelper.AssertIsNotNull(deferrer, realTimeDeferrer);

            Deferrer = deferrer;
            RealTimeDeferrer = realTimeDeferrer;
        }
    }
}