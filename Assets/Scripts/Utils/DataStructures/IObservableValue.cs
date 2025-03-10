using System;

namespace BattleCruisers.Utils.DataStrctures
{
    public interface IObservableValue<T>
    {
        T Value { get; }

        event EventHandler ValueChanged;
    }
}