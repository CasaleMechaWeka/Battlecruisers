using BattleCruisers.Utils.Properties;
using BattleCruisers.UI.Panels;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Panels
{
    public interface IPvPSlidingPanel : IPanel
    {
        PvPPanelState TargetState { get; }
        IBroadcastingProperty<PvPPanelState> State { get; }
    }
}