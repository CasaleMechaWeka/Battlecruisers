using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Boost
{
    public class GlobalBoostProviders : IGlobalBoostProviders
    {
        public IObservableCollection<IBoostProvider> DummyBoostProviders { get; private set; }
        public IObservableCollection<IBoostProvider> AircraftBoostProviders { get; private set; }
        public IObservableCollection<IBoostProvider> TurretAccuracyBoostProviders { get; private set; }
        public IObservableCollection<IBoostProvider> TurretFireRateBoostProviders { get; private set; }
        public IObservableCollection<IBoostProvider> DefenseFireRateBoostProviders { get; private set; }
        public IObservableCollection<IBoostProvider> OffenseFireRateBoostProviders { get; private set; }

        public GlobalBoostProviders()
        {
            DummyBoostProviders = new DummyObservableCollection<IBoostProvider>();
            AircraftBoostProviders = new ObservableCollection<IBoostProvider>();
            TurretAccuracyBoostProviders = new ObservableCollection<IBoostProvider>();
            TurretFireRateBoostProviders = new ObservableCollection<IBoostProvider>();
            DefenseFireRateBoostProviders = new ObservableCollection<IBoostProvider>();
            OffenseFireRateBoostProviders = new ObservableCollection<IBoostProvider>();
        }
	}
}
