using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders
{
    public class PvPBuildingHealthlBoostProviders : IBuildingHealthlBoostProviders
    {
        public ObservableCollection<IBoostProvider> AllBuildingsProviders { get; }

        public PvPBuildingHealthlBoostProviders()
        {
            AllBuildingsProviders = new ObservableCollection<IBoostProvider>();
        }
    }
}
