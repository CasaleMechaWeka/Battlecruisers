using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Data.Models
{
    // FELIX  Interface, test, test serialization!
    [Serializable]
    public class NewItems<TItem> where TItem : PrefabKey
    {
        [SerializeField]
        private List<TItem> _items;

        public ReadOnlyCollection<TItem> Items { get; }

        public NewItems()
        {
            _items = new List<TItem>();
            Items = _items.AsReadOnly();
        }

        public void AddItem(TItem newItem)
        {
            _items.Add(newItem);
        }

        public bool RemoveItem(TItem oldItem)
        {
            return _items.Remove(oldItem);
        }

        public override bool Equals(object obj)
        {
            return
                obj is NewItems<TItem> other
                && Enumerable.SequenceEqual(Items, other.Items);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(Items);
        }
    }
}