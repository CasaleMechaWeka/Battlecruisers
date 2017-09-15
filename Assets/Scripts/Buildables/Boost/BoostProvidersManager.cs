using System.Collections.Generic;
using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Boost
{
    public class BoostProvidersManager : IBoostProvidersManager
    {
        public IObservableCollection<IBoostProvider> AircraftBoostProviders { get; private set; }

        public BoostProvidersManager()
        {
            AircraftBoostProviders = new ObservableCollection<IBoostProvider>(new List<IBoostProvider>());
        }
	}
}
