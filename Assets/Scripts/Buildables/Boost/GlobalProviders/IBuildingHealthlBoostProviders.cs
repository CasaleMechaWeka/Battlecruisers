using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Boost.GlobalProviders
{
    public interface IBuildingHealthlBoostProviders
    {
        ObservableCollection<IBoostProvider> AllBuildingsProviders { get; }
    }
}
