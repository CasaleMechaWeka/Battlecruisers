using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units
{
    public enum PvPUnitCategory
    {
        Naval, Aircraft, Untouchable
    }

    public enum PvPDirection
    {
        Left, Right, Up, Down
    }

    public interface IPvPUnit : IPvPBuildable, IPvPRemovable, IPvPPoolable<PvPBuildableActivationArgs>
    {
        PvPUnitCategory Category { get; }
        IPvPDroneConsumerProvider DroneConsumerProvider { set; }
        PvPDirection FacingDirection { get; }
        float MaxVelocityInMPerS { get; }
        bool IsUltra { get; }

        void AddBuildRateBoostProviders(ObservableCollection<IPvPBoostProvider> boostProviders);
    }
}
