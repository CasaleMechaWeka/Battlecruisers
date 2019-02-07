using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Utils.DataStrctures
{
    public class DummyObservableCollection<T> : IObservableCollection<T>
    {
        public ReadOnlyCollection<T> Items { get; private set; }

        public event EventHandler<CollectionChangedEventArgs<T>> Changed;

        public DummyObservableCollection()
        {
            Items = new ReadOnlyCollection<T>(new List<T>());
        }

        public void Add(T item)
        {
            // empty
        }

        public void Insert(int index, T item)
        {
            // empty
        }

        public bool Remove(T item)
        {
            return false;
        }
    }
}