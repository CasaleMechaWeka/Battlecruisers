using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Panels
{
    public interface IPvPSlidingPanel : IPvPPanel
    {
        PvPPanelState TargetState { get; }
        IPvPBroadcastingProperty<PvPPanelState> State { get; }
    }
}