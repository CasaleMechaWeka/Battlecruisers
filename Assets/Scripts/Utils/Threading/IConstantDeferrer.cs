using System;

namespace BattleCruisers.Utils.Threading
{
    public interface IConstantDeferrer
    {
        void Defer(Action action);
    }
}