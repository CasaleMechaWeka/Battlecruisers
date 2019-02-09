using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Boost.GlobalProviders
{
    public class GlobalBoostProviders : IGlobalBoostProviders
    {
        // The BoostableGroup does not allow the same IBoostProviders collection
        // to be added twice, even if it is just the same DummyBoostProviders :/
        // Hence create fresh instead of returning the same instance.
        public IObservableCollection<IBoostProvider> DummyBoostProviders { get { return new DummyObservableCollection<IBoostProvider>(); } }

        public IObservableCollection<IBoostProvider> AircraftBoostProviders { get; private set; }

        public IObservableCollection<IBoostProvider> TurretAccuracyBoostProviders { get; private set; }
        public IObservableCollection<IBoostProvider> DefenseFireRateBoostProviders { get; private set; }
        public IObservableCollection<IBoostProvider> OffenseFireRateBoostProviders { get; private set; }
        
        public IObservableCollection<IBoostProvider> ShieldRechargeRateBoostProviders { get; private set; }

        public IBuildingBuildRatelBoostProviders BuildingBuildRate { get; private set; }

        public GlobalBoostProviders()
        {
            AircraftBoostProviders = new ObservableCollection<IBoostProvider>();

            DefenseFireRateBoostProviders = new ObservableCollection<IBoostProvider>();
            OffenseFireRateBoostProviders = new ObservableCollection<IBoostProvider>();
            TurretAccuracyBoostProviders = new ObservableCollection<IBoostProvider>();

            ShieldRechargeRateBoostProviders = new ObservableCollection<IBoostProvider>();

            BuildingBuildRate = new BuildingBuildRatelBoostProviders();
        }
	}
}
