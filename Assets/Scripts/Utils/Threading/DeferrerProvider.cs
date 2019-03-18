using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Threading
{
    public class DeferrerProvider : IDeferrerProvider
    {
        public IDeferrer Deferrer { get; }

        public DeferrerProvider(IDeferrer deferrer)
        {
            Assert.IsNotNull(deferrer);
            Deferrer = deferrer;
        }
    }
}