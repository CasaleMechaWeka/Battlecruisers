using System;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost
{
    /// <summary>
    /// That combines the boost multipliers from:
    ///     * lists of boost providers ([][])
    /// And applies the overall boost to:
    ///     * boostables
    /// </summary>
    public interface IPvPBoostableGroup
    {
        event EventHandler BoostChanged;

        void AddBoostable(IPvPBoostable boostable);
        bool RemoveBoostable(IPvPBoostable boostable);

        void AddBoostProvidersList(ObservableCollection<IPvPBoostProvider> boostProviders);

        void CleanUp();
    }
}

