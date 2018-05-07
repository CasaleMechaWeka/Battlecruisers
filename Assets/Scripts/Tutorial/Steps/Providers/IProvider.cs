namespace BattleCruisers.Tutorial.Steps.Providers
{
    /// <summary>
    /// Allows a reference to the provider to be used BEFORE the actual item
    /// the provider will return is available.
    /// 
    /// Ie, the LastFriendlyBuildingStartedProvider would return the last
    /// building started by the player.
    /// </summary>
    /// FELIX  Only used for IBuildable :/  Just make IBuildableProvider interface?
    public interface IProvider<T>
    {
        T FindItem();
    }
}
