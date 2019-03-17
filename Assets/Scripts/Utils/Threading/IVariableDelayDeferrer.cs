using System;

namespace BattleCruisers.Utils.Threading
{
    // FELIX  Rename to IDeferrer :)
    public interface IVariableDelayDeferrer
    {
        void Defer(Action action, float delayInS);
    }
}
