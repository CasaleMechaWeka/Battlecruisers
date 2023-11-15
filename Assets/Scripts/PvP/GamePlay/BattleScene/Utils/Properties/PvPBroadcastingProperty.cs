using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties
{
    public class PvPBroadcastingProperty<T> : IPvPBroadcastingProperty<T>
    {
        private readonly IPvPSettableBroadcastingProperty<T> _baseProperty;

        public T Value => _baseProperty.Value;

        public event EventHandler ValueChanged
        {
            add { _baseProperty.ValueChanged += value; }
            remove { _baseProperty.ValueChanged -= value; }
        }

        public PvPBroadcastingProperty(IPvPSettableBroadcastingProperty<T> baseProperty)
        {
            Assert.IsNotNull(baseProperty);
            _baseProperty = baseProperty;
        }
    }
}