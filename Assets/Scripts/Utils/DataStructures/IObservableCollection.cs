namespace BattleCruisers.Utils.DataStrctures
{
    /// <summary>
    /// Unity does not have ObservableCollection, so have to implement my own :)
    /// </summary>
    /// FELIX  Update tests :P
    public interface IObservableCollection<T> : IReadonlyObservableCollection<T>
    {
        void Add(T item);
        void Insert(int index, T item);
        bool Remove(T item);
    }
}
