using System;

namespace UnityCommon.Properties
{
    public interface IBroadcastingProperty<T>
    {
        T Value { get; }

        event EventHandler ValueChanged;
    }
}