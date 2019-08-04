using System;

namespace BattleCruisers.Utils.Events
{
    public interface IDebouncable<TEventArgs> where TEventArgs : EventArgs
    {
        event EventHandler<TEventArgs> UndebouncedEvent;

        void EmitDebouncedEvent(TEventArgs eventArgs);
    }
}