using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders
{
    public class PvPGlobalBoostProviders : IPvPGlobalBoostProviders
    {
        // The BoostableGroup does not allow the same IBoostProviders collection
        // to be added twice, even if it is just the same DummyBoostProviders :/
        // Hence create fresh instance instead of returning the same instance.
        public ObservableCollection<IBoostProvider> DummyBoostProviders => new ObservableCollection<IBoostProvider>();

        public ObservableCollection<IBoostProvider> AircraftBoostProviders { get; }

        public ObservableCollection<IBoostProvider> TurretAccuracyBoostProviders { get; }
        public ObservableCollection<IBoostProvider> DefenseFireRateBoostProviders { get; }
        public ObservableCollection<IBoostProvider> OffenseFireRateBoostProviders { get; }

        public ObservableCollection<IBoostProvider> ShieldRechargeRateBoostProviders { get; }
        public ObservableCollection<IBoostProvider> RocketBuildingsFireRateBoostProviders { get; }
        public ObservableCollection<IBoostProvider> RocketBuildingsDamageBoostProviders { get; }

        public IPvPBuildingBuildRatelBoostProviders BuildingBuildRate { get; }
        public IUnitBuildRatelBoostProviders UnitBuildRate { get; }
        public IPvPBuildingHealthlBoostProviders BuildingHealth { get; }

        public PvPGlobalBoostProviders()
        {
            AircraftBoostProviders = new ObservableCollection<IBoostProvider>();

            DefenseFireRateBoostProviders = new ObservableCollection<IBoostProvider>();
            OffenseFireRateBoostProviders = new ObservableCollection<IBoostProvider>();
            TurretAccuracyBoostProviders = new ObservableCollection<IBoostProvider>();

            ShieldRechargeRateBoostProviders = new ObservableCollection<IBoostProvider>();

            RocketBuildingsFireRateBoostProviders = new ObservableCollection<IBoostProvider>();

            BuildingBuildRate = new PvPBuildingBuildRatelBoostProviders();
            UnitBuildRate = new PvPUnitBuildRatelBoostProviders();
            BuildingHealth = new PvPBuildingHealthlBoostProviders();
        }
    }
}
