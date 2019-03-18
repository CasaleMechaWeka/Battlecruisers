using System;
using System.Threading.Tasks;

namespace BattleCruisers.Utils.Threading
{
    public class Deferrer : IDeferrer
    {
        public async void Defer(Action action, float delayInS)
        {
            await Task.Delay(TimeSpan.FromSeconds(delayInS));
            action.Invoke();
        }
    }
}
