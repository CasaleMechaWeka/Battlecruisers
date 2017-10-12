using System;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Tests
{
    public class DummyDeferrer : IDeferrer
    {
        public void Defer(Action action)
        {
            action.Invoke();
        }
    }
}