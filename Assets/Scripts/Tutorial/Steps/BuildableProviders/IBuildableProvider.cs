using BattleCruisers.Buildables;

namespace BattleCruisers.Tutorial.Steps.BuildableProviders
{
    /// <summary>
    /// Allows a reference to the provider to be used BEFORE the actual buildable
    /// the provider will return is available.
    /// 
    /// Ie, the LastFriendlyBuildingStartedProvider would return the last
    /// building started by the player.
    /// </summary>
    public interface IBuildableProvider
    {
        IBuildable Buildable { get; }
    }
}
