using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using System.Collections.ObjectModel;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units
{
    public interface IPvPUnit : IPvPBuildable, IRemovable, IPvPPoolable<PvPBuildableActivationArgs>
    {
        UnitCategory Category { get; }
        IDroneConsumerProvider DroneConsumerProvider { set; }
        Direction FacingDirection { get; }
        float MaxVelocityInMPerS { get; }
        bool IsUltra { get; }

        void AddBuildRateBoostProviders(ObservableCollection<IPvPBoostProvider> boostProviders);

        int variantIndex { get; set; }
        void ApplyVariantStats(StatVariant statVariant);

        float YSpawnOffset { get; }
    }
}
