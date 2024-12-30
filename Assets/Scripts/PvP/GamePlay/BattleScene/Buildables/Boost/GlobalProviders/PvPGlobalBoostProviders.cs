using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders
{
    public class PvPGlobalBoostProviders : IPvPGlobalBoostProviders
    {
        // The BoostableGroup does not allow the same IBoostProviders collection
        // to be added twice, even if it is just the same DummyBoostProviders :/
        // Hence create fresh instance instead of returning the same instance.
        public ObservableCollection<IPvPBoostProvider> DummyBoostProviders => new ObservableCollection<IPvPBoostProvider>();

        public ObservableCollection<IPvPBoostProvider> AircraftBoostProviders { get; }

        public ObservableCollection<IPvPBoostProvider> TurretAccuracyBoostProviders { get; }
        public ObservableCollection<IPvPBoostProvider> DefenseFireRateBoostProviders { get; }
        public ObservableCollection<IPvPBoostProvider> OffenseFireRateBoostProviders { get; }

        public ObservableCollection<IPvPBoostProvider> ShieldRechargeRateBoostProviders { get; }
        public ObservableCollection<IPvPBoostProvider> RocketBuildingsFireRateBoostProviders { get; }
        public ObservableCollection<IPvPBoostProvider> RocketBuildingsDamageBoostProviders { get; }

        public IPvPBuildingBuildRatelBoostProviders BuildingBuildRate { get; }
        public IPvPUnitBuildRatelBoostProviders UnitBuildRate { get; }
        public IPvPBuildingHealthlBoostProviders BuildingHealth { get; }

        public PvPGlobalBoostProviders()
        {
            AircraftBoostProviders = new ObservableCollection<IPvPBoostProvider>();

            DefenseFireRateBoostProviders = new ObservableCollection<IPvPBoostProvider>();
            OffenseFireRateBoostProviders = new ObservableCollection<IPvPBoostProvider>();
            TurretAccuracyBoostProviders = new ObservableCollection<IPvPBoostProvider>();

            ShieldRechargeRateBoostProviders = new ObservableCollection<IPvPBoostProvider>();

            RocketBuildingsFireRateBoostProviders = new ObservableCollection<IPvPBoostProvider>();

            BuildingBuildRate = new PvPBuildingBuildRatelBoostProviders();
            UnitBuildRate = new PvPUnitBuildRatelBoostProviders();
            BuildingHealth = new PvPBuildingHealthlBoostProviders();
        }
    }
}
