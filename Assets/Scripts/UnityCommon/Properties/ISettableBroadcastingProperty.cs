using System;

namespace UnityCommon.Properties
{
    public interface ISettableBroadcastingProperty<T>
    {
        T Value { get; set; }

        event EventHandler ValueChanged;
    }
}