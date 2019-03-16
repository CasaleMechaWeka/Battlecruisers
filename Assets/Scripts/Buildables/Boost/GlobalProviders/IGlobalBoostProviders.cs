using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Boost.GlobalProviders
{
    public interface IGlobalBoostProviders
    {
        ObservableCollection<IBoostProvider> DummyBoostProviders { get; }

        ObservableCollection<IBoostProvider> AircraftBoostProviders { get; }

        ObservableCollection<IBoostProvider> DefenseFireRateBoostProviders { get; }
        ObservableCollection<IBoostProvider> OffenseFireRateBoostProviders { get; }
        // Currently affects ALL turrets (ships, gunship, buildings).  
        // That's ok though, because only used in tutorial to improve artillery accuracy :)
        ObservableCollection<IBoostProvider> TurretAccuracyBoostProviders { get; }

        ObservableCollection<IBoostProvider> ShieldRechargeRateBoostProviders { get; }

        IBuildingBuildRatelBoostProviders BuildingBuildRate { get; }
        IUnitBuildRatelBoostProviders UnitBuildRate { get; }
    }
}
