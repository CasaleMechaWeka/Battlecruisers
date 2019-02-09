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

        IBuildingBuildRatelBoostProviders BuildingBuildRate { get; }
    }
}
