using System;
using System.Collections.Generic;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties
{
    public class PvPSettableBroadcastingProperty<T> : ISettableBroadcastingProperty<T>
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

        public PvPSettableBroadcastingProperty(T initialValue)
        {
            _value = initialValue;
        }
    }
}