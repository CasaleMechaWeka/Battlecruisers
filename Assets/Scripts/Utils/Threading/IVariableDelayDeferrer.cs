using System;

namespace BattleCruisers.Utils.Threading
{
    public interface IVariableDelayDeferrer
    {
        void Defer(Action action, float delayInS);
    }
}
