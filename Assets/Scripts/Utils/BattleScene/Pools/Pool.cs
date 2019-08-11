using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene.Pools
{
    // FELIX  Use
    // FELIX  Test
    // FELIX  Have initial capacity?  Need to deactivate items...  But they have not been Activated/Initialised?
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

        public IPoolable<TArgs> GetItem(TArgs initialisationArgs)
        {
            IPoolable<TArgs> item = _items.Count != 0 ? _items.Pop() : _itemFactory.CreateItem();
            item.Activate(initialisationArgs);
            return item;
        }

        public void ReleaseItem(IPoolable<TArgs> itemToRelease)
        {
            itemToRelease.Deactivate();
            _items.Push(itemToRelease);
        }
    }
}