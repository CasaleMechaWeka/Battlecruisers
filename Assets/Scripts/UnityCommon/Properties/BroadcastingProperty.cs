using System;
using UnityEngine.Assertions;

namespace UnityCommon.Properties
{
    public class BroadcastingProperty<T> : IBroadcastingProperty<T>
    {
        private readonly ISettableBroadcastingProperty<T> _baseProperty;

        public T Value => _baseProperty.Value;

        public event EventHandler ValueChanged
        {
            add { _baseProperty.ValueChanged += value; }
            remove { _baseProperty.ValueChanged -= value; }
        }

        public BroadcastingProperty(ISettableBroadcastingProperty<T> baseProperty)
        {
            Assert.IsNotNull(baseProperty);
            _baseProperty = baseProperty;
        }
    }
}