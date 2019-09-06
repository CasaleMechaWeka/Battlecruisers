using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene.Pools
{
    public class Pool<TPoolable, TArgs> : IPool<TPoolable, TArgs> where TPoolable : class, IPoolable<TArgs>
    {
        private readonly Stack<TPoolable> _items;
        private readonly IPoolableFactory<TPoolable, TArgs> _itemFactory;
        private int _createCount = 0;

        public Pool(IPoolableFactory<TPoolable, TArgs> itemFactory, int initialCapacity)
        {
            Assert.IsNotNull(itemFactory);

            _itemFactory = itemFactory;
            _items = new Stack<TPoolable>();

            for (int i = 0; i < initialCapacity; ++i)
            {
                _items.Push(itemFactory.CreateItem());
            }
        }

        public TPoolable GetItem(TArgs activationArgs)
        {
            Logging.LogMethod(Tags.POOLS);

            TPoolable item = _items.Count != 0 ? _items.Pop() : CreateItem();
            item.Activate(activationArgs);
            return item;
        }

        private TPoolable CreateItem()
        {
            Logging.Log(Tags.POOLS, $"{_itemFactory} {++_createCount}");

            TPoolable item = _itemFactory.CreateItem();
            item.Deactivated += Item_Deactivated;
            return item;
        }

        private void Item_Deactivated(object sender, EventArgs e)
        {
            Logging.LogMethod(Tags.POOLS);
            _items.Push(sender.Parse<TPoolable>());
        }
    }
}