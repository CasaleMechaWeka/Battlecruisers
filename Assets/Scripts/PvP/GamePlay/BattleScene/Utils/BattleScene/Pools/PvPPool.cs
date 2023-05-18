using System;
using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools
{
    public class PvPPool<TPoolable, TArgs> : IPvPPool<TPoolable, TArgs> where TPoolable : class, IPvPPoolable<TArgs>
    {
        private readonly Stack<TPoolable> _items;
        private readonly IPvPPoolableFactory<TPoolable, TArgs> _itemFactory;
        private int _createCount = 0;
        private int MaxLimit = 1000;

        public PvPPool(IPvPPoolableFactory<TPoolable, TArgs> itemFactory)
        {
            Assert.IsNotNull(itemFactory);

            _itemFactory = itemFactory;
            _items = new Stack<TPoolable>();
        }

        public void AddCapacity(int capacityToAdd)
        {
            // Logging.Verbose(Tags.POOLS, $"{typeof(TPoolable)}:  {capacityToAdd} items");

            for (int i = 0; i < capacityToAdd; ++i)
            {
                _items.Push(_itemFactory.CreateItem());
            }
        }

        public TPoolable GetItem(TArgs activationArgs)
        {
            if (_items.Count < MaxLimit)
            {
                TPoolable item = _items.Count != 0 ? _items.Pop() : CreateItem();
                // Logging.Verbose(Tags.POOLS, $"{item}");

                item.Activate(activationArgs);
                return item;
            }
            return null;
        }

        public TPoolable GetItem(TArgs activationArgs, PvPFaction faction)
        {
            TPoolable item = _items.Count != 0 ? _items.Pop() : CreateItem();
            // Logging.Verbose(Tags.POOLS, $"{item}");

            item.Activate(activationArgs, faction);
            return item;
        }

        private TPoolable CreateItem()
        {
            // Logging.Verbose(Tags.POOLS, $"{typeof(TPoolable)}: {_itemFactory} {++_createCount}");

            TPoolable item = _itemFactory.CreateItem();
            item.Deactivated += Item_Deactivated;
            return item;
        }

        private void Item_Deactivated(object sender, EventArgs e)
        {
            // Logging.Verbose(Tags.POOLS, $"{typeof(TPoolable)}");
            _items.Push(sender.Parse<TPoolable>());
        }

        public void SetMaxLimit(int amount)
        {
            MaxLimit = amount;
        }
    }
}