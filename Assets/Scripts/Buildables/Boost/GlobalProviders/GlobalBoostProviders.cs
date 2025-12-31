using System.Collections.Generic;
using System.Collections.ObjectModel;
using static BattleCruisers.Buildables.Boost.GlobalProviders.BoostType;

namespace BattleCruisers.Buildables.Boost.GlobalProviders
{
    public enum BoostType   // DO NOT CHANGE THE VALUES, or you break all cruiser prefabs!!!
    {                       // 1st digit == kind of bonus; last 2 digits == boost group
        AccuracyAllBuilding     = 002,
        
        BuildRateAircraft       = 100,
        BuildRateAirFactory     = 101,
        BuildRateAllBuilding    = 102,
        BuildRateDefense        = 103,
        BuildRateDrone          = 104,
        BuildRateMast           = 105,
        BuildRateOffense        = 106,
        BuildRateRocket         = 107,
        BuildRateSeaFactory     = 108,
        BuildRateShield         = 109,
        BuildRateShip           = 110,
        BuildRateTactical       = 111,
        BuildRateTacticalUltra  = 112,
        BuildRateUltra          = 113,
        
        FireRateDefense         = 203,
        FireRateOffense         = 206,
        FireRateRocket          = 207,
        
        HealthAllBuilding       = 302,

        RechargeRateShield      = 409,
    }
    
    public class GlobalBoostProviders
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

        public BuildingBuildRatelBoostProviders BuildingBuildRate { get; }
        public UnitBuildRatelBoostProviders UnitBuildRate { get; }

        public BuildingHealthlBoostProviders BuildingHealth { get; }

        public Dictionary<string, SpecializedBuildableModifiers> SpecializedBuildableBoosts { get; }

        public ObservableCollection<IBoostProvider> BoostTypeToBoostProvider(BoostType boostType)
        {
            return boostType switch
            {
                BuildRateAircraft       => UnitBuildRate.AircraftProviders,
                BuildRateAllBuilding    => BuildingBuildRate.AllBuildingsProviders,
                BuildRateShip           => UnitBuildRate.ShipProviders,
                BuildRateUltra          => BuildingBuildRate.UltrasProviders,
                BuildRateMast           => BuildingBuildRate.MastStructureProviders,
                BuildRateShield         => BuildingBuildRate.ShieldsProviders,
                BuildRateDefense        => BuildingBuildRate.DefensivesProviders,
                BuildRateDrone          => BuildingBuildRate.DroneBuildingsProviders,
                BuildRateSeaFactory     => BuildingBuildRate.NavalFactoryProviders,
                BuildRateAirFactory     => BuildingBuildRate.AirFactoryProviders,
                BuildRateRocket         => BuildingBuildRate.RocketBuildingsProviders,
                BuildRateTactical       => BuildingBuildRate.TacticalsProviders,
                BuildRateTacticalUltra  => BuildingBuildRate.TacticalUltrasProviders,
                BuildRateOffense        => BuildingBuildRate.OffensivesProviders,
                
                FireRateRocket          => RocketBuildingsFireRateBoostProviders,
                FireRateDefense         => DefenseFireRateBoostProviders,
                FireRateOffense         => OffenseFireRateBoostProviders,
                
                HealthAllBuilding       => BuildingHealth.AllBuildingsProviders,
                
                AccuracyAllBuilding     => TurretAccuracyBoostProviders,
                
                RechargeRateShield      => ShieldRechargeRateBoostProviders,
                
                _                       => throw new System.NotImplementedException(),
            };
        }

        public GlobalBoostProviders()
        {
            AircraftBoostProviders = new ObservableCollection<IBoostProvider>();

            DefenseFireRateBoostProviders = new ObservableCollection<IBoostProvider>();
            OffenseFireRateBoostProviders = new ObservableCollection<IBoostProvider>();
            TurretAccuracyBoostProviders = new ObservableCollection<IBoostProvider>();

            ShieldRechargeRateBoostProviders = new ObservableCollection<IBoostProvider>();

            RocketBuildingsFireRateBoostProviders = new ObservableCollection<IBoostProvider>();

            BuildingBuildRate = new BuildingBuildRatelBoostProviders();
            UnitBuildRate = new UnitBuildRatelBoostProviders();

            BuildingHealth = new BuildingHealthlBoostProviders();

            SpecializedBuildableBoosts = new Dictionary<string, SpecializedBuildableModifiers>();
        }
    }
}
