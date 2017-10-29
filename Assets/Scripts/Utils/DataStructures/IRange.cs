namespace BattleCruisers.Utils.DataStrctures
{
    public interface IRange<T>
    {
        T Min { get; }
        T Max { get; }
    }
}
