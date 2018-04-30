namespace BattleCruisers.Tutorial.Steps.Providers
{
    /// <summary>
    /// Allows a reference to the provider to be used BEFORE the actual item
    /// the provider will return is available.
    /// 
    /// Ie, the LastFriendlyBuildingStartedProvider would return the last
    /// building started by the player.
    /// </summary>
    public interface IProvider<T>
    {
        T Item { get; }
    }
}
