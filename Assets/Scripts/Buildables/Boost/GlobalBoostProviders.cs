using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Boost
{
    public class GlobalBoostProviders : IGlobalBoostProviders
    {
        public IObservableCollection<IBoostProvider> DummyBoostProviders { get; private set; }

        public IObservableCollection<IBoostProvider> AircraftBoostProviders { get; private set; }

        public IObservableCollection<IBoostProvider> TurretAccuracyBoostProviders { get; private set; }
        public IObservableCollection<IBoostProvider> DefenseFireRateBoostProviders { get; private set; }
        public IObservableCollection<IBoostProvider> OffenseFireRateBoostProviders { get; private set; }
        
        public IObservableCollection<IBoostProvider> ShieldRechargeRateBoostProviders { get; private set; }

        // Build rate
        public IObservableCollection<IBoostProvider> DefensivesBuildRateBoostProviders { get; private set; }
        public IObservableCollection<IBoostProvider> OffensivesBuildRateBoostProviders { get; private set; }
        public IObservableCollection<IBoostProvider> TacticalsBuildRateBoostProviders { get; private set; }
        public IObservableCollection<IBoostProvider> ShieldsBuildRateBoostProviders { get; private set; }
        public IObservableCollection<IBoostProvider> UltrasBuildRateBoostProviders { get; private set; }
        public IObservableCollection<IBoostProvider> AirFactoryBuildRateBoostProviders { get; private set; }
        public IObservableCollection<IBoostProvider> NavalFactoryBuildRateBoostProviders { get; private set; }
        public IObservableCollection<IBoostProvider> DroneBuildingsBuildRateBoostProviders { get; private set; }

        public GlobalBoostProviders()
        {
            DummyBoostProviders = new DummyObservableCollection<IBoostProvider>();

            AircraftBoostProviders = new ObservableCollection<IBoostProvider>();

            DefenseFireRateBoostProviders = new ObservableCollection<IBoostProvider>();
            OffenseFireRateBoostProviders = new ObservableCollection<IBoostProvider>();
            TurretAccuracyBoostProviders = new ObservableCollection<IBoostProvider>();

            ShieldRechargeRateBoostProviders = new ObservableCollection<IBoostProvider>();

            // Build rate
            DefenseFireRateBoostProviders = new ObservableCollection<IBoostProvider>();
            OffensivesBuildRateBoostProviders = new ObservableCollection<IBoostProvider>();
            TacticalsBuildRateBoostProviders = new ObservableCollection<IBoostProvider>();
            ShieldsBuildRateBoostProviders = new ObservableCollection<IBoostProvider>();
            UltrasBuildRateBoostProviders = new ObservableCollection<IBoostProvider>();
            AircraftBoostProviders = new ObservableCollection<IBoostProvider>();
            NavalFactoryBuildRateBoostProviders = new ObservableCollection<IBoostProvider>();
            DroneBuildingsBuildRateBoostProviders = new ObservableCollection<IBoostProvider>();
        }
	}
}
