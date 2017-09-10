using System;
using System.Collections.ObjectModel;

namespace BattleCruisers.Utils.DataStrctures
{
    public enum ChangeType
    {
        Add, Remove
    }

    public class CollectionChangedEventArgs<T> : EventArgs
    {
        public ChangeType Type { get; private set; }
        public T Item { get; private set; }

        public CollectionChangedEventArgs(ChangeType type, T item)
        {
            Type = type;
            Item = item;
        }
    }

    /// <summary>
    /// Unity does not have ObservableCollection, so have to implement my own :)
    /// </summary>
    public interface IObservableCollection<T>
    {
        ReadOnlyCollection<T> Items { get; }

        event EventHandler<CollectionChangedEventArgs<T>> Changed;

        void Add(T item);
        void Remove(T item);
    }
}
