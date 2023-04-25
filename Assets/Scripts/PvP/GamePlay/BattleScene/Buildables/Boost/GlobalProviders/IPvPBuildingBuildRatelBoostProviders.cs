using System.Collections.ObjectModel;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders
{
    public interface IPvPBuildingBuildRatelBoostProviders
    {
        ObservableCollection<IPvPBoostProvider> DefensivesProviders { get; }
        ObservableCollection<IPvPBoostProvider> OffensivesProviders { get; }
        ObservableCollection<IPvPBoostProvider> TacticalsProviders { get; }
        ObservableCollection<IPvPBoostProvider> ShieldsProviders { get; }
        ObservableCollection<IPvPBoostProvider> UltrasProviders { get; }
        ObservableCollection<IPvPBoostProvider> AirFactoryProviders { get; }
        ObservableCollection<IPvPBoostProvider> NavalFactoryProviders { get; }
        // Drone stations and Ultralisks
        ObservableCollection<IPvPBoostProvider> DroneBuildingsProviders { get; }
    }
}