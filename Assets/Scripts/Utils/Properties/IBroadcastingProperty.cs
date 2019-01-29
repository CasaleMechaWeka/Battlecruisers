using System;

namespace BattleCruisers.Utils.Properties
{
    public interface IBroadcastingProperty<T>
    {
        T Value { get; }

        event EventHandler ValueChanged;
    }
}