using BattleCruisers.Utils.BattleScene.Pools;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Utils.BattleScene.Pools
{
    public class PoolTests
    {
        private IPool<IPoolable<int>, int> _pool;
        private IPoolableFactory<IPoolable<int>, int> _itemFactory;
        private IPoolable<int> _item;

        [SetUp]
        public void TestSetup()
        {
            _itemFactory = Substitute.For<IPoolableFactory<IPoolable<int>, int>>();

            _pool = new Pool<IPoolable<int>, int>(_itemFactory);

            _item = Substitute.For<IPoolable<int>>();
            _itemFactory.CreateItem().Returns(_item);
        }

        [Test]
        public void GetItem_NoStoredItems_CreatesItem()
        {
            IPoolable<int> returnedItem = _pool.GetItem(7);

            Assert.AreSame(_item, returnedItem);
            _itemFactory.Received().CreateItem();
            _item.Received().Activate(7);
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
            _item.Received().Activate(12);
        }
    }
}