namespace BattleCruisers.Utils.DataStrctures
{
    /// <summary>
    /// Unity does not have ObservableCollection, so have to implement my own :)
    /// </summary>
    public interface IObservableCollection<T> : IReadonlyObservableCollection<T>
    {
        void Add(T item);
        bool Remove(T item);
    }
}
