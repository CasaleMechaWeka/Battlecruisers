using BattleCruisers.Buildables.Boost;
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

        void AddBoostable(IBoostable boostable);
        bool RemoveBoostable(IBoostable boostable);

        void AddBoostProvidersList(ObservableCollection<IBoostProvider> boostProviders);

        void CleanUp();
    }
}

