using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Threading
{
    public class DeferrerProvider : IDeferrerProvider
    {
        public IVariableDelayDeferrer VariableDelayDeferrer { get; }

        public DeferrerProvider(IVariableDelayDeferrer variableDelayDeferrer)
        {
            Assert.IsNotNull(variableDelayDeferrer);
            VariableDelayDeferrer = variableDelayDeferrer;
        }
    }
}