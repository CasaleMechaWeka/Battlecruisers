using System;
using System.Threading.Tasks;

namespace BattleCruisers.Utils.Threading
{
    // FELIX  Rename to Deferrer :)
    public class VariableDelayDeferrer : IVariableDelayDeferrer
    {
        public async void Defer(Action action, float delayInS)
        {
            await Task.Delay(TimeSpan.FromSeconds(delayInS));
            action.Invoke();
        }
    }
}
