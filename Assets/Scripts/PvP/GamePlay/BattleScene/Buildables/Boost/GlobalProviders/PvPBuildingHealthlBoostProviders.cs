using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders
{
    public class PvPBuildingHealthlBoostProviders : IPvPBuildingHealthlBoostProviders
    {
        public ObservableCollection<IPvPBoostProvider> AllBuildingsProviders { get; }

        public PvPBuildingHealthlBoostProviders()
        {
            AllBuildingsProviders = new ObservableCollection<IPvPBoostProvider>();
        }
    }
}
