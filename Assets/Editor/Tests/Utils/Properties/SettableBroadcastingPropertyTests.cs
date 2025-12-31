using BattleCruisers.Utils.Properties;
using NUnit.Framework;

namespace BattleCruisers.Tests.Utils.Properties
{
    public class SettableBroadcastingPropertyTests
    {
        private ISettableBroadcastingProperty<int> _property;
        private int _changeEventCounter;

        [SetUp]
        public void TestSetup()
        {
            _property = new SettableBroadcastingProperty<int>(-7);

            _changeEventCounter = 0;
            _property.ValueChanged += (sender, e) => _changeEventCounter++;
        }

        [Test]
        public void InitialValue()
        {
            Assert.AreEqual(-7, _property.Value);
        }

        [Test]
        public void SetValue_SameValue_NoEvent()
        {
            _property.Value = _property.Value;

            Assert.AreEqual(0, _changeEventCounter);
            Assert.AreEqual(-7, _property.Value);
        }

        [Test]
        public void SetValue_NewValue_EmitsEvent()
        {
            _property.Value = 12;

            Assert.AreEqual(1, _changeEventCounter);
            Assert.AreEqual(12, _property.Value);
        }
    }
}