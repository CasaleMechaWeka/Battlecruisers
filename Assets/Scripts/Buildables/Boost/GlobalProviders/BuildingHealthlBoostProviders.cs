using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Boost.GlobalProviders
{
    public class BuildingHealthlBoostProviders
    {
        public ObservableCollection<IBoostProvider> AllBuildingsProviders { get; }

        public BuildingHealthlBoostProviders()
        {
            AllBuildingsProviders = new ObservableCollection<IBoostProvider>();
        }
    }
}
