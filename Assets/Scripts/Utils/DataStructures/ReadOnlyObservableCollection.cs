using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.DataStrctures
{
    public class ReadOnlyObservableCollection<T> : IReadOnlyObservableCollection<T>
    {
        private readonly IList<T> _items;
        public ReadOnlyCollection<T> Items { get; private set; }

        public event EventHandler<CollectionChangedEventArgs<T>> Changed;

        public ReadOnlyObservableCollection(IList<T> items)
        {
            Assert.IsNotNull(items);

            _items = items;
            Items = new ReadOnlyCollection<T>(_items);
        }

        public void Add(T item)
        {
            _items.Add(item);
            EmitChangedEvent(ChangeType.Add, item);
        }

        public void Remove(T item)
        {
            bool wasSuccessful = _items.Remove(item);

            if (wasSuccessful)
            {
                EmitChangedEvent(ChangeType.Remove, item);
            }
        }

        private void EmitChangedEvent(ChangeType type, T item)
        {
            if (Changed != null)
            {
                Changed.Invoke(this, new CollectionChangedEventArgs<T>(type, item));
            }
        }
    }
}
