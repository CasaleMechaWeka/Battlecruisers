using System;

namespace BattleCruisers.Utils.Timers
{
    public interface IDebouncer
    {
        void Debounce(Action action);
    }
}