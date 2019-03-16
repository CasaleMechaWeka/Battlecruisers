using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Boost.GlobalProviders
{
    public class BuildingBuildRatelBoostProviders : IBuildingBuildRatelBoostProviders
    {
        public ObservableCollection<IBoostProvider> DefensivesProviders { get; private set; }
        public ObservableCollection<IBoostProvider> OffensivesProviders { get; private set; }
        public ObservableCollection<IBoostProvider> TacticalsProviders { get; private set; }
        public ObservableCollection<IBoostProvider> ShieldsProviders { get; private set; }
        public ObservableCollection<IBoostProvider> UltrasProviders { get; private set; }
        public ObservableCollection<IBoostProvider> AirFactoryProviders { get; private set; }
        public ObservableCollection<IBoostProvider> NavalFactoryProviders { get; private set; }
        public ObservableCollection<IBoostProvider> DroneBuildingsProviders { get; private set; }

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
