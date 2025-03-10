using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models
{
    [Serializable]
    public class PvPNewItems<TItem> where TItem : PvPPrefabKey
    {
        [SerializeField]
        private ObservableCollection<TItem> _items;

        public ReadOnlyObservableCollection<TItem> Items { get; }

        public PvPNewItems()
        {
            _items = new ObservableCollection<TItem>();
            Items = new ReadOnlyObservableCollection<TItem>(_items);
        }

        public void AddItem(TItem newItem)
        {
            Logging.LogMethod(Tags.MODELS);
            _items.Add(newItem);
        }

        public bool RemoveItem(TItem oldItem)
        {
            Logging.LogMethod(Tags.MODELS);
            return _items.Remove(oldItem);
        }

        public override bool Equals(object obj)
        {
            return
                obj is PvPNewItems<TItem> other
                && Enumerable.SequenceEqual(Items, other.Items);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(Items);
        }
    }
}