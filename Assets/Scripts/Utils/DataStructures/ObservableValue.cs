using System;
using System.Collections.Generic;

namespace BattleCruisers.Utils.DataStrctures
{
    public class ObservableValue<T> : IObservableValue<T>
    {
        private T _value;
        public T Value
        {
            get { return _value; }
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    _value = value;

                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler ValueChanged;

        public ObservableValue(T initialValue)
        {
            _value = initialValue;
        }
    }
}