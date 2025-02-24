using BattleCruisers.Buildables.Boost;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders
{
    public interface IPvPBuildingHealthlBoostProviders
    {
        ObservableCollection<IBoostProvider> AllBuildingsProviders { get; }
    }
}
