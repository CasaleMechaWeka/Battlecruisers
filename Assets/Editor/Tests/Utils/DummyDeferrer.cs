using System;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Tests
{
    public class DummyDeferrer : IDeferrer
    {
        public void DeferToFrameEnd(Action action)
        {
            action.Invoke();
        }
    }
}