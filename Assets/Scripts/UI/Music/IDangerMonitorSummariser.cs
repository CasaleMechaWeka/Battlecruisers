using UnityCommon.Properties;

namespace BattleCruisers.UI.Music
{
    public interface IDangerMonitorSummariser
    {
        IBroadcastingProperty<bool> IsInDanger { get; }
    }
}