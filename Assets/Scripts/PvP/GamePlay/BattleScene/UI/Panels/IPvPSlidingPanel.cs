using BattleCruisers.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Panels
{
    public interface IPvPSlidingPanel : IPvPPanel
    {
        PvPPanelState TargetState { get; }
        IBroadcastingProperty<PvPPanelState> State { get; }
    }
}