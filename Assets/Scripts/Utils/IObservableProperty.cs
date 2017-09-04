using System;

namespace BattleCruisers.Utils
{
    public interface IObservableProperty<T>
    {
        T Value { get; }

        event EventHandler PropertyChanged;
    }
}
