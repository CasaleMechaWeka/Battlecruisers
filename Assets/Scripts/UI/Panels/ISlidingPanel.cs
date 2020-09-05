using UnityCommon.Properties;

namespace BattleCruisers.UI.Panels
{
    public interface ISlidingPanel 
    { 
        IBroadcastingProperty<PanelState> State { get; }
    }
}