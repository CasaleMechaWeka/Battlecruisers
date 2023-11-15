using System;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures
{
    public class PvPObservableValue<T> : IPvPObservableValue<T>
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

        public PvPObservableValue(T initialValue)
        {
            _value = initialValue;
        }
    }
}