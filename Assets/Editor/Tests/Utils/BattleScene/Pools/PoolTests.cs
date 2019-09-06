using BattleCruisers.Utils.BattleScene.Pools;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Utils.BattleScene.Pools
{
    public class PoolTests
    {
        private IPool<IPoolable<int>, int> _pool;
        private IPoolableFactory<IPoolable<int>, int> _itemFactory;
        private IPoolable<int> _item1, _item2, _item3;

        [SetUp]
        public void TestSetup()
        {
            _itemFactory = Substitute.For<IPoolableFactory<IPoolable<int>, int>>();

            _item1 = Substitute.For<IPoolable<int>>();
            _item2 = Substitute.For<IPoolable<int>>();
            _item3 = Substitute.For<IPoolable<int>>();
            _itemFactory.CreateItem().Returns(_item1, _item2, _item3);

            _pool = new Pool<IPoolable<int>, int>(_itemFactory, initialCapacity: 0);
        }

        [Test]
        public void CreateInitialCapacity()
        {
            _pool = new Pool<IPoolable<int>, int>(_itemFactory, initialCapacity: 2);
            _itemFactory.Received(2).CreateItem();
            _itemFactory.ClearReceivedCalls();
        }

        [Test]
        public void GetItem_NoStoredItems_CreatesItem()
        {
            IPoolable<int> returnedItem = _pool.GetItem(7);

            Assert.AreSame(_item1, returnedItem);
            _itemFactory.Received().CreateItem();
            _item1.Received().Activate(7);
        }

        [Test]
        public void GetItem_HasStoredItems_ReturnsStoredItem()
        {
            // First retrieval => create item
            IPoolable<int> firstItem = _pool.GetItem(7);

            // Item deactivated => store item in pool
            firstItem.Deactivated += Raise.Event();

            // Second retrieval => returns stored item
            _itemFactory.ClearReceivedCalls();

            IPoolable<int> secondItem = _pool.GetItem(12);

            Assert.AreSame(firstItem, secondItem);
            _itemFactory.DidNotReceive().CreateItem();
            _item1.Received().Activate(12);
        }

        [Test]
        public void GetItem_OnceInitialCapacityIsExhausted()
        {
            _pool = new Pool<IPoolable<int>, int>(_itemFactory, initialCapacity: 2);
            _itemFactory.ClearReceivedCalls();

            // Get first stored item
            IPoolable<int> firstItem = _pool.GetItem(62);
            Assert.AreSame(_item2, firstItem);

            // Get second stored item
            IPoolable<int> secondItem = _pool.GetItem(626);
            Assert.AreSame(_item1, secondItem);

            // Should have retrieved stored items, no need to create new items
            _itemFactory.DidNotReceive().CreateItem();

            // Get new item, as initial capacity has run out
            IPoolable<int> thirdItem = _pool.GetItem(71);
            Assert.AreSame(_item3, thirdItem);
            _itemFactory.Received().CreateItem();
        }
    }
}