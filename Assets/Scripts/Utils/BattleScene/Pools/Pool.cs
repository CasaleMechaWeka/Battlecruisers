using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene.Pools
{
    public class Pool<TArgs> : IPool<TArgs>
    {
        private readonly Stack<IPoolable<TArgs>> _items;
        private readonly IPoolableFactory<TArgs> _itemFactory;

        public Pool(IPoolableFactory<TArgs> itemFactory)
        {
            Assert.IsNotNull(itemFactory);

            _itemFactory = itemFactory;
            _items = new Stack<IPoolable<TArgs>>();
        }

        public IPoolable<TArgs> GetItem(TArgs activationArgs)
        {
            Logging.LogMethod(Tags.POOLS);

            IPoolable<TArgs> item = _items.Count != 0 ? _items.Pop() : CreateItem();
            item.Activate(activationArgs);
            return item;
        }

        private IPoolable<TArgs> CreateItem()
        {
            Logging.LogMethod(Tags.POOLS);

            IPoolable<TArgs> item = _itemFactory.CreateItem();
            item.Deactivated += Item_Deactivated;
            return item;
        }

        private void Item_Deactivated(object sender, EventArgs e)
        {
            Logging.LogMethod(Tags.POOLS);

            _items.Push(sender.Parse<IPoolable<TArgs>>());
        }
    }
}