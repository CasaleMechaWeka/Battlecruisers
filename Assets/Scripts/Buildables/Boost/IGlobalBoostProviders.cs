using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Boost
{
    public interface IGlobalBoostProviders
    {
        IObservableCollection<IBoostProvider> AircraftBoostProviders { get; }

        // FELIX  Currently affects ALL turrets (ships, gunship, buildings).  Use more restrictive boosters :)
        IObservableCollection<IBoostProvider> TurretAccuracyBoostProviders { get; }
        IObservableCollection<IBoostProvider> TurretFireRateBoostProviders { get; }

        // FELIX  Use :D
        IObservableCollection<IBoostProvider> DefenseFireRateBoostProviders { get; }
        IObservableCollection<IBoostProvider> OffenseFireRateBoostProviders { get; }
    }
}
