using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools
{
    public class PvPPool<TPoolable, TArgs> : IPvPPool<TPoolable, TArgs> where TPoolable : class, IPoolable<TArgs>
    {
        private readonly Stack<TPoolable> _items;
        private readonly IPvPPoolableFactory<TPoolable, TArgs> _itemFactory;
        private int MaxLimit = 1000;

        public PvPPool(IPvPPoolableFactory<TPoolable, TArgs> itemFactory)
        {
            Assert.IsNotNull(itemFactory);
            _itemFactory = itemFactory;
            _items = new Stack<TPoolable>();
        }

        public void AddCapacity(int capacityToAdd)
        {
            for (int i = 0; i < capacityToAdd; ++i)
            {
                var item = _itemFactory.CreateItem();
                _items.Push(item);
            }
        }

        public TPoolable GetItem(TArgs activationArgs)
        {
            if (_items.Count < MaxLimit)
            {
                TPoolable item = _items.Count != 0 ? _items.Pop() : CreateItem();
                item.Activate(activationArgs);
                return item;
            }
            return null;
        }

        public TPoolable GetItem(TArgs activationArgs, Faction faction)
        {
            TPoolable item = _items.Count != 0 ? _items.Pop() : CreateItem();
            item.Activate(activationArgs, faction);
            return item;
        }

        private TPoolable CreateItem()
        {
            TPoolable item = _itemFactory.CreateItem();
            item.Deactivated += Item_Deactivated;
            return item;
        }

        private void Item_Deactivated(object sender, EventArgs e)
        {
            _items.Push(sender.Parse<TPoolable>());
        }

        public void SetMaxLimit(int amount)
        {
            MaxLimit = amount;
        }
    }
}