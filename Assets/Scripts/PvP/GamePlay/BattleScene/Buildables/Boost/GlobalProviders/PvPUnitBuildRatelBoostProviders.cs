using BattleCruisers.Buildables.Boost;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders
{
    public class PvPUnitBuildRatelBoostProviders : IPvPUnitBuildRatelBoostProviders
    {
        public ObservableCollection<IBoostProvider> AircraftProviders { get; }
        public ObservableCollection<IBoostProvider> ShipProviders { get; }

        public PvPUnitBuildRatelBoostProviders()
        {
            AircraftProviders = new ObservableCollection<IBoostProvider>();
            ShipProviders = new ObservableCollection<IBoostProvider>();
        }
    }
}
