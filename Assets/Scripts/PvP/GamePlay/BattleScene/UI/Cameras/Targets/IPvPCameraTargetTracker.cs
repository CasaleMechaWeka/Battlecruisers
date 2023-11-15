using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets
{
    public interface IPvPCameraTargetTracker
    {
        IPvPBroadcastingProperty<bool> IsOnTarget { get; }
    }
}