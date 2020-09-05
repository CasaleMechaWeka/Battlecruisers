using UnityCommon.Properties;

namespace BattleCruisers.UI.Panels
{
    public interface ISlidingPanel : IPanel
    { 
        IBroadcastingProperty<PanelState> State { get; }
    }
}