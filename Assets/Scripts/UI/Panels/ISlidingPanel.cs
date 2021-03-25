using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.Panels
{
    public interface ISlidingPanel : IPanel
    { 
        PanelState TargetState { get; }
        IBroadcastingProperty<PanelState> State { get; }
    }
}