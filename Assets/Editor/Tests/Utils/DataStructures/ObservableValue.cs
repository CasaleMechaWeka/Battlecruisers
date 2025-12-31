using BattleCruisers.Utils.DataStrctures;
using NUnit.Framework;

namespace BattleCruisers.Tests.Utils.DataStructures
{
    public class ObservableValueTests
    {
        private ObservableValue<bool> _boolValue;
        private ObservableValue<object> _objectValue;
        private object _obj1, _obj2;
        private int _valueTypeChangeCount, _referenceTypeChangeCount;

        [SetUp]
        public void TestSetup()
        {
            _boolValue = new ObservableValue<bool>(true);
            _valueTypeChangeCount = 0;
            _boolValue.ValueChanged += (sender, e) => _valueTypeChangeCount++;

            _obj1 = new object();
            _obj2 = new object();
            _objectValue = new ObservableValue<object>(_obj1);
            _referenceTypeChangeCount = 0;
            _objectValue.ValueChanged += (sender, e) => _referenceTypeChangeCount++;
        }

        [Test]
        public void GetReturnsValue()
        {
            Assert.IsTrue(_boolValue.Value);
            Assert.AreSame(_obj1, _objectValue.Value);
        }

        [Test]
        public void ValueType_SetSameValue_EmitsChangedEvent()
        {
            _boolValue.Value = true;
            Assert.AreEqual(0, _valueTypeChangeCount);
        }

        [Test]
        public void ValueType_SetDifferentValue_EmitsChangedEvent()
        {
            _boolValue.Value = false;
            Assert.AreEqual(1, _valueTypeChangeCount);
            Assert.IsFalse(_boolValue.Value);
        }

        [Test]
        public void ReferenceType_SetSameValue_EmitsChangedEvent()
        {
            _objectValue.Value = _obj1;
            Assert.AreEqual(0, _referenceTypeChangeCount);
        }

        [Test]
        public void ReferenceType_SetDifferentValue_EmitsChangedEvent()
        {
            _objectValue.Value = _obj2;
            Assert.AreEqual(1, _referenceTypeChangeCount);
            Assert.AreSame(_obj2, _objectValue.Value);
        }
    }
}