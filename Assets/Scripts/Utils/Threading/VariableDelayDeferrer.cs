using System;
using System.Threading.Tasks;

namespace BattleCruisers.Utils.Threading
{
    public class VariableDelayDeferrer : IVariableDelayDeferrer
    {
        public async void Defer(Action action, float delayInS)
        {
            await Task.Delay(TimeSpan.FromSeconds(delayInS));
            action.Invoke();
        }
    }
}
