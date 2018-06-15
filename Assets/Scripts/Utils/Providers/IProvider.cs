namespace BattleCruisers.Utils.Providers
{
    public interface IProvider<T>
    {
        T Value { get; }
    }
}
