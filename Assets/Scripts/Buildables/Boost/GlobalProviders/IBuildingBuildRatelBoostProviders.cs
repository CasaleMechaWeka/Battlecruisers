using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Boost.GlobalProviders
{
    public interface IBuildingBuildRatelBoostProviders
    {
        ObservableCollection<IBoostProvider> DefensivesProviders { get; }
        ObservableCollection<IBoostProvider> OffensivesProviders { get; }
        ObservableCollection<IBoostProvider> TacticalsProviders { get; }
        ObservableCollection<IBoostProvider> ShieldsProviders { get; }
        ObservableCollection<IBoostProvider> UltrasProviders { get; }
        ObservableCollection<IBoostProvider> AirFactoryProviders { get; }
        ObservableCollection<IBoostProvider> NavalFactoryProviders { get; }
        // Drone stations and Ultralisks
        ObservableCollection<IBoostProvider> DroneBuildingsProviders { get; }
    }
}
