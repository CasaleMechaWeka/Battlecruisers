using System;

namespace BattleCruisers.Utils.Properties
{
    public interface ISettableBroadcastingProperty<T>
    {
        T Value { get; set; }

        event EventHandler ValueChanged;
    }
}