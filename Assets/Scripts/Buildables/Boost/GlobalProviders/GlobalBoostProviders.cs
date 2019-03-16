using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Boost.GlobalProviders
{
    public class GlobalBoostProviders : IGlobalBoostProviders
    {
        // The BoostableGroup does not allow the same IBoostProviders collection
        // to be added twice, even if it is just the same DummyBoostProviders :/
        // Hence create fresh instance instead of returning the same instance.
        public ObservableCollection<IBoostProvider> DummyBoostProviders { get { return new ObservableCollection<IBoostProvider>(); } }

        public ObservableCollection<IBoostProvider> AircraftBoostProviders { get; private set; }

        public ObservableCollection<IBoostProvider> TurretAccuracyBoostProviders { get; private set; }
        public ObservableCollection<IBoostProvider> DefenseFireRateBoostProviders { get; private set; }
        public ObservableCollection<IBoostProvider> OffenseFireRateBoostProviders { get; private set; }
        
        public ObservableCollection<IBoostProvider> ShieldRechargeRateBoostProviders { get; private set; }

        public IBuildingBuildRatelBoostProviders BuildingBuildRate { get; private set; }
        public IUnitBuildRatelBoostProviders UnitBuildRate { get; private set; }

        public GlobalBoostProviders()
        {
            AircraftBoostProviders = new ObservableCollection<IBoostProvider>();

            DefenseFireRateBoostProviders = new ObservableCollection<IBoostProvider>();
            OffenseFireRateBoostProviders = new ObservableCollection<IBoostProvider>();
            TurretAccuracyBoostProviders = new ObservableCollection<IBoostProvider>();

            ShieldRechargeRateBoostProviders = new ObservableCollection<IBoostProvider>();

            BuildingBuildRate = new BuildingBuildRatelBoostProviders();
            UnitBuildRate = new UnitBuildRatelBoostProviders();
        }
	}
}
