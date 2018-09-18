using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Threading
{
    public class DeferrerProvider : IDeferrerProvider
    {
        public IVariableDelayDeferrer VariableDelayDeferrer { get; private set; }

        public DeferrerProvider(IVariableDelayDeferrer variableDelayDeferrer)
        {
            Assert.IsNotNull(variableDelayDeferrer);
            VariableDelayDeferrer = variableDelayDeferrer;
        }
    }
}