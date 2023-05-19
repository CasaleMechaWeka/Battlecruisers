using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders
{
    public class PvPBuildingBuildRatelBoostProviders : IPvPBuildingBuildRatelBoostProviders
    {
        public ObservableCollection<IPvPBoostProvider> DefensivesProviders { get; }
        public ObservableCollection<IPvPBoostProvider> OffensivesProviders { get; }
        public ObservableCollection<IPvPBoostProvider> TacticalsProviders { get; }
        public ObservableCollection<IPvPBoostProvider> ShieldsProviders { get; }
        public ObservableCollection<IPvPBoostProvider> UltrasProviders { get; }
        public ObservableCollection<IPvPBoostProvider> AirFactoryProviders { get; }
        public ObservableCollection<IPvPBoostProvider> NavalFactoryProviders { get; }
        public ObservableCollection<IPvPBoostProvider> DroneBuildingsProviders { get; }

        public PvPBuildingBuildRatelBoostProviders()
        {
            DefensivesProviders = new ObservableCollection<IPvPBoostProvider>();
            OffensivesProviders = new ObservableCollection<IPvPBoostProvider>();
            TacticalsProviders = new ObservableCollection<IPvPBoostProvider>();
            ShieldsProviders = new ObservableCollection<IPvPBoostProvider>();
            UltrasProviders = new ObservableCollection<IPvPBoostProvider>();
            AirFactoryProviders = new ObservableCollection<IPvPBoostProvider>();
            NavalFactoryProviders = new ObservableCollection<IPvPBoostProvider>();
            DroneBuildingsProviders = new ObservableCollection<IPvPBoostProvider>();
        }
    }
}
