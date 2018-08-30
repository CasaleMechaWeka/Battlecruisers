using BattleCruisers.Utils.DataStrctures;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Utils.DataStructures
{
    public class ObservableCollectionTests
	{
        private IObservableCollection<object> _collection;
        private object _item;
        private CollectionChangedEventArgs<object> _lastEventArgs;

		[SetUp]
		public void SetuUp()
		{
            _collection = new ObservableCollection<object>();
			_collection.Changed += (sender, e) => _lastEventArgs = e;
            _item = new object();
            _lastEventArgs = null;

			UnityAsserts.Assert.raiseExceptions = true;
		}

		[Test]
		public void Add_AddsAndEmitsChangedEvent()
		{
            _collection.Add(_item);

            Assert.AreEqual(new CollectionChangedEventArgs<object>(ChangeType.Add, _item), _lastEventArgs);
            Assert.IsTrue(_collection.Items.Contains(_item));
		}

        [Test]
        public void Insert_InsertsAndEmitsChangedEvent()
        {
            _collection.Insert(0, _item);

            Assert.AreEqual(new CollectionChangedEventArgs<object>(ChangeType.Add, _item), _lastEventArgs);
            Assert.AreSame(_item, _collection.Items[0]);
        }

        [Test]
        public void Remove_Removes_EmitsChangedEvent_AndReturnsTrue()
        {
            _collection.Add(_item);
            bool wasSuccessful = _collection.Remove(_item);

            Assert.IsTrue(wasSuccessful);
            Assert.AreEqual(new CollectionChangedEventArgs<object>(ChangeType.Remove, _item), _lastEventArgs);
            Assert.IsFalse(_collection.Items.Contains(_item));
        }

        [Test]
        public void Remove_NonExistantItem_ReturnsFalse()
        {
			bool wasSuccessful = _collection.Remove(_item);

            Assert.IsFalse(wasSuccessful);
            Assert.IsNull(_lastEventArgs);
            Assert.AreEqual(0, _collection.Items.Count);
        }
	}
}
