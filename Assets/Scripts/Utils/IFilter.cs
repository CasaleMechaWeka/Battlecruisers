namespace BattleCruisers.Utils
{
    public interface IFilter<TElement>
    {
        bool IsMatch(TElement element);
    }
}
