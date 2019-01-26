using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.Utils.Properties
{
    public class BroadcastingProperty : IBroadcastingProperty<TargetType?>
    {
        private TargetType? _value;
        public TargetType? Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
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