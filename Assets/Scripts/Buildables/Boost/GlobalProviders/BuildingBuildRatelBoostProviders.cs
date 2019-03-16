using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Boost.GlobalProviders
{
    public class BuildingBuildRatelBoostProviders : IBuildingBuildRatelBoostProviders
    {
        public ObservableCollection<IBoostProvider> DefensivesProviders { get; }
        public ObservableCollection<IBoostProvider> OffensivesProviders { get; }
        public ObservableCollection<IBoostProvider> TacticalsProviders { get; }
        public ObservableCollection<IBoostProvider> ShieldsProviders { get; }
        public ObservableCollection<IBoostProvider> UltrasProviders { get; }
        public ObservableCollection<IBoostProvider> AirFactoryProviders { get; }
        public ObservableCollection<IBoostProvider> NavalFactoryProviders { get; }
        public ObservableCollection<IBoostProvider> DroneBuildingsProviders { get; }

        public BuildingBuildRatelBoostProviders()
        {
            DefensivesProviders = new ObservableCollection<IBoostProvider>();
            OffensivesProviders = new ObservableCollection<IBoostProvider>();
            TacticalsProviders = new ObservableCollection<IBoostProvider>();
            ShieldsProviders = new ObservableCollection<IBoostProvider>();
            UltrasProviders = new ObservableCollection<IBoostProvider>();
            AirFactoryProviders = new ObservableCollection<IBoostProvider>();
            NavalFactoryProviders = new ObservableCollection<IBoostProvider>();
            DroneBuildingsProviders = new ObservableCollection<IBoostProvider>();
        }
	}
}
