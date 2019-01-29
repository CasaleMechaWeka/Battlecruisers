using System;
using System.Collections.Generic;

namespace BattleCruisers.Utils.Properties
{
    // FELIX  Test :D
    public class SettableBroadcastingProperty<T> : ISettableBroadcastingProperty<T>
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

                    if (ValueChanged != null)
                    {
                        ValueChanged.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        public event EventHandler ValueChanged;
    }
}