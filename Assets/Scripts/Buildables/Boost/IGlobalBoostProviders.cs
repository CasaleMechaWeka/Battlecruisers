using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Boost
{
    public interface IGlobalBoostProviders
    {
        IObservableCollection<IBoostProvider> DummyBoostProviders { get; }
        IObservableCollection<IBoostProvider> AircraftBoostProviders { get; }
        IObservableCollection<IBoostProvider> DefenseFireRateBoostProviders { get; }
        IObservableCollection<IBoostProvider> OffenseFireRateBoostProviders { get; }

        // FELIX  Currently affects ALL turrets (ships, gunship, buildings).  Use more restrictive boosters :)
        IObservableCollection<IBoostProvider> TurretAccuracyBoostProviders { get; }
        IObservableCollection<IBoostProvider> TurretFireRateBoostProviders { get; }
    }
}
