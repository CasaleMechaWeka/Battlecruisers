using System;

namespace BattleCruisers.Utils.Threading
{
    public class DummyConstantDeferrer : IConstantDeferrer
    {
        public void Defer(Action action)
        {
            action.Invoke();
        }
    }
}