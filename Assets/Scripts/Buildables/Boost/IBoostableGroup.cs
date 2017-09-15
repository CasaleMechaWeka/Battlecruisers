using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Boost
{
    /// <summary>
    /// Coordinates:
    ///     + 1 boost consumer
    /// That combines the boost multipliers from:
    ///     * lists of boost providers ([][])
    /// And applies the overall boost to:
    ///     * boostables
    /// </summary>
    public interface IBoostableGroup
    {
        IBoostConsumer BoostConsumer { get; }

        void AddBoostable(IBoostable boostable);
        bool RemoveBoostable(IBoostable boostable);

        void AddBoostProvidersList(IObservableCollection<IBoostProvider> boostProviders);

        void CleanUp();
    }
}
