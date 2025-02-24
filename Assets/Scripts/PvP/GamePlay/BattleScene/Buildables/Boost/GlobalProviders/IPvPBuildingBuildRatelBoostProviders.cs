using BattleCruisers.Buildables.Boost;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders
{
    public interface IPvPBuildingBuildRatelBoostProviders
    {
        ObservableCollection<IBoostProvider> DefensivesProviders { get; }
        ObservableCollection<IBoostProvider> OffensivesProviders { get; }
        ObservableCollection<IBoostProvider> TacticalsProviders { get; }
        ObservableCollection<IBoostProvider> ShieldsProviders { get; }
        ObservableCollection<IBoostProvider> UltrasProviders { get; }
        ObservableCollection<IBoostProvider> AirFactoryProviders { get; }
        ObservableCollection<IBoostProvider> NavalFactoryProviders { get; }
        ObservableCollection<IBoostProvider> RocketBuildingsProviders { get; }
        // Drone stations and Ultralisks
        ObservableCollection<IBoostProvider> DroneBuildingsProviders { get; }
        ObservableCollection<IBoostProvider> AllBuildingsProviders { get; }
        ObservableCollection<IBoostProvider> TacticalUltrasProviders { get; }
        ObservableCollection<IBoostProvider> MastStructureProviders { get; }
    }
}