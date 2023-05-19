using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders
{
    public class PvPUnitBuildRatelBoostProviders : IPvPUnitBuildRatelBoostProviders
    {
        public ObservableCollection<IPvPBoostProvider> AircraftProviders { get; }
        public ObservableCollection<IPvPBoostProvider> ShipProviders { get; }

        public PvPUnitBuildRatelBoostProviders()
        {
            AircraftProviders = new ObservableCollection<IPvPBoostProvider>();
            ShipProviders = new ObservableCollection<IPvPBoostProvider>();
        }
    }
}
