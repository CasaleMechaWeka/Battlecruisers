using BattleCruisers.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets
{
    public interface IPvPCameraTargetTracker
    {
        IBroadcastingProperty<bool> IsOnTarget { get; }
    }
}