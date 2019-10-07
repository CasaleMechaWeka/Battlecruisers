using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using NUnit.Framework;
using System.Collections.Specialized;

namespace BattleCruisers.Tests.Data.Models
{
    public class NewItemsTests
    {
        private NewItems<HullKey> _newItems;
        private HullKey _hullKey;
        private int _changedCount;

        [SetUp]
        public void TestSetup()
        {
            _newItems = new NewItems<HullKey>();
            _hullKey = StaticPrefabKeys.Hulls.Longbow;

            _changedCount = 0;
            ((INotifyCollectionChanged)_newItems.Items).CollectionChanged += (sender, e) => _changedCount++;
        }

        [Test]
        public void AddItem()
        {
            _newItems.AddItem(_hullKey);

            Assert.IsTrue(_newItems.Items.Contains(_hullKey));
            Assert.AreEqual(1, _changedCount);
        }

        [Test]
        public void RemoveItem()
        {
            // Add item
            _newItems.AddItem(_hullKey);

            Assert.IsTrue(_newItems.Items.Contains(_hullKey));
            Assert.AreEqual(1, _changedCount);

            // Remove item
            _newItems.RemoveItem(_hullKey);

            Assert.IsFalse(_newItems.Items.Contains(_hullKey));
            Assert.AreEqual(2, _changedCount);
        }
    }
}