using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Boost.GlobalProviders
{
    public class GlobalBoostProviders : IGlobalBoostProviders
    {
        // The BoostableGroup does not allow the same IBoostProviders collection
        // to be added twice, even if it is just the same DummyBoostProviders :/
        // Hence create fresh instance instead of returning the same instance.
        public ObservableCollection<IBoostProvider> DummyBoostProviders { get { return new ObservableCollection<IBoostProvider>(); } }

        public ObservableCollection<IBoostProvider> AircraftBoostProviders { get; }

        public ObservableCollection<IBoostProvider> TurretAccuracyBoostProviders { get; }
        public ObservableCollection<IBoostProvider> DefenseFireRateBoostProviders { get; }
        public ObservableCollection<IBoostProvider> OffenseFireRateBoostProviders { get; }
        
        public ObservableCollection<IBoostProvider> ShieldRechargeRateBoostProviders { get; }

        public IBuildingBuildRatelBoostProviders BuildingBuildRate { get; }
        public IUnitBuildRatelBoostProviders UnitBuildRate { get; }

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
