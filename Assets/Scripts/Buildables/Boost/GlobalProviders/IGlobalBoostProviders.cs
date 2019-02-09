using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Boost.GlobalProviders
{
    public interface IGlobalBoostProviders
    {
        IObservableCollection<IBoostProvider> DummyBoostProviders { get; }

        IObservableCollection<IBoostProvider> AircraftBoostProviders { get; }

        IObservableCollection<IBoostProvider> DefenseFireRateBoostProviders { get; }
        IObservableCollection<IBoostProvider> OffenseFireRateBoostProviders { get; }
        // Currently affects ALL turrets (ships, gunship, buildings).  
        // That's ok though, because only used in tutorial to improve artillery accuracy :)
        IObservableCollection<IBoostProvider> TurretAccuracyBoostProviders { get; }

        IObservableCollection<IBoostProvider> ShieldRechargeRateBoostProviders { get; }

        // Build rate
        IObservableCollection<IBoostProvider> DefensivesBuildRateBoostProviders { get; }
        IObservableCollection<IBoostProvider> OffensivesBuildRateBoostProviders { get; }
        IObservableCollection<IBoostProvider> TacticalsBuildRateBoostProviders { get; }
        IObservableCollection<IBoostProvider> ShieldsBuildRateBoostProviders { get; }
        IObservableCollection<IBoostProvider> UltrasBuildRateBoostProviders { get; }
        IObservableCollection<IBoostProvider> AirFactoryBuildRateBoostProviders { get; }
        IObservableCollection<IBoostProvider> NavalFactoryBuildRateBoostProviders { get; }
        // Drone stations and Ultralisks
        IObservableCollection<IBoostProvider> DroneBuildingsBuildRateBoostProviders { get; }
    }
}
