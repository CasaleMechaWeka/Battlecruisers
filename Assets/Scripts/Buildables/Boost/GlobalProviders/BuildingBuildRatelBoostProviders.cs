using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Boost.GlobalProviders
{
    public class BuildingBuildRatelBoostProviders : IBuildingBuildRatelBoostProviders
    {
        public IObservableCollection<IBoostProvider> DefensivesProviders { get; private set; }
        public IObservableCollection<IBoostProvider> OffensivesProviders { get; private set; }
        public IObservableCollection<IBoostProvider> TacticalsProviders { get; private set; }
        public IObservableCollection<IBoostProvider> ShieldsProviders { get; private set; }
        public IObservableCollection<IBoostProvider> UltrasProviders { get; private set; }
        public IObservableCollection<IBoostProvider> AirFactoryProviders { get; private set; }
        public IObservableCollection<IBoostProvider> NavalFactoryProviders { get; private set; }
        public IObservableCollection<IBoostProvider> DroneBuildingsProviders { get; private set; }

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
