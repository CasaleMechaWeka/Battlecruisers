using System;

namespace BattleCruisers.Utils
{
    public class ObservableProperty<T> : IObservableProperty<T>
	{
        private T _value;
		public T Value 
        { 
            get { return _value; }
            set
            {
                if (!value.Equals(_value))
                {
                    _value = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

		public event EventHandler PropertyChanged;
	}
}
