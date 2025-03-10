namespace BattleCruisers.Utils.Threading
{
    public class DeferrerProvider : IDeferrerProvider
    {
        public IDeferrer Deferrer { get; }
        public IDeferrer RealTimeDeferrer { get; }

        public DeferrerProvider(IDeferrer deferrer, IDeferrer realTimeDeferrer)
        {
            Helper.AssertIsNotNull(deferrer, realTimeDeferrer);

            Deferrer = deferrer;
            RealTimeDeferrer = realTimeDeferrer;
        }
    }
}