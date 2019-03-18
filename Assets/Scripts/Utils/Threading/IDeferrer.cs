using System;

namespace BattleCruisers.Utils.Threading
{
    public interface IDeferrer
    {
        void Defer(Action action, float delayInS);
    }
}
