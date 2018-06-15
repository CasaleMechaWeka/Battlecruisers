using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Boost
{
    public interface IGlobalBoostProviders
    {
        IObservableCollection<IBoostProvider> AircraftBoostProviders { get; }
        IObservableCollection<IBoostProvider> TurretAccuracyBoostProviders { get; }
        IObservableCollection<IBoostProvider> TurretFireRateBoostProviders { get; }
	}
}
