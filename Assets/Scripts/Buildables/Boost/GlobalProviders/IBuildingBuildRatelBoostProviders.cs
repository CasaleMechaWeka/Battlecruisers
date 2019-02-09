using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Boost.GlobalProviders
{
    public interface IBuildingBuildRatelBoostProviders
    {
        IObservableCollection<IBoostProvider> DefensivesProviders { get; }
        IObservableCollection<IBoostProvider> OffensivesProviders { get; }
        IObservableCollection<IBoostProvider> TacticalsProviders { get; }
        IObservableCollection<IBoostProvider> ShieldsProviders { get; }
        IObservableCollection<IBoostProvider> UltrasProviders { get; }
        IObservableCollection<IBoostProvider> AirFactoryProviders { get; }
        IObservableCollection<IBoostProvider> NavalFactoryProviders { get; }
        // Drone stations and Ultralisks
        IObservableCollection<IBoostProvider> DroneBuildingsProviders { get; }
    }
}
