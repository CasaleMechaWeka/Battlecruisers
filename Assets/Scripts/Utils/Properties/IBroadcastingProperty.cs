using System;

namespace BattleCruisers.Utils.Properties
{
    public interface IBroadcastingProperty<T>
    {
        T Value { get; set; }

        event EventHandler ValueChanged;
    }
}