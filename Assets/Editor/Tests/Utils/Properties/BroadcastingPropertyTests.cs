using UnityCommon.Properties;
using NSubstitute;
using NUnit.Framework;
using System;

namespace BattleCruisers.Tests.Utils.Properties
{
    public class BroadcastingPropertyTests
    {
        private IBroadcastingProperty<int> _property;
        private ISettableBroadcastingProperty<int> _baseProperty;
        private int _valueChangedCount;

        [SetUp]
        public void TestSetup()
        {
            _baseProperty = Substitute.For<ISettableBroadcastingProperty<int>>();
            _property = new BroadcastingProperty<int>(_baseProperty);

            _valueChangedCount = 0;
        }

        [Test]
        public void Value_ForwardsToBaseProperty()
        {
            _baseProperty.Value.Returns(-7);
            Assert.AreEqual(_baseProperty.Value, _property.Value);
        }

        [Test]
        public void ValueChanged_ForwardsToBaseEvent()
        {
            // Subscribing
            _property.ValueChanged += _property_ValueChanged;
            _baseProperty.ValueChanged += Raise.Event();
            Assert.AreEqual(1, _valueChangedCount);

            // Unsubscribing
            _property.ValueChanged -= _property_ValueChanged;
            _baseProperty.ValueChanged += Raise.Event();
            Assert.AreEqual(1, _valueChangedCount);
        }

        private void _property_ValueChanged(object sender, EventArgs e)
        {
            _valueChangedCount++;
        }
    }
}