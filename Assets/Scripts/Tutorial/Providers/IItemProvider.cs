namespace BattleCruisers.Tutorial.Providers
{
    /// <summary>
    /// Allows a reference to the provider to be used BEFORE the actual item
    /// the provider will return is available.
    /// </summary>
    public interface IItemProvider<TItem>
    {
        TItem FindItem();
    }
}
