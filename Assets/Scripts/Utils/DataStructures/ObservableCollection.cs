using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Utils.DataStrctures
{
    public class ObservableCollection<T> : IObservableCollection<T>
    {
        private readonly IList<T> _items;
        public ReadOnlyCollection<T> Items { get; private set; }

        public event EventHandler<CollectionChangedEventArgs<T>> Changed;

        public ObservableCollection()
        {
            _items = new List<T>();
            Items = new ReadOnlyCollection<T>(_items);
        }

        public void Add(T item)
        {
            _items.Add(item);
            EmitChangedEvent(ChangeType.Add, item);
        }

        public bool Remove(T item)
        {
            bool wasSuccessful = _items.Remove(item);

            if (wasSuccessful)
            {
                EmitChangedEvent(ChangeType.Remove, item);
            }

            return wasSuccessful;
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
