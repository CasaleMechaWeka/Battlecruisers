using System;

namespace BattleCruisers.AI.ThreatMonitors
{
    // Do not change enum order, as the values are used for comparisons.
    // Ie:  myThreatLevel > ThreatLevel.Low
    public enum ThreatLevel
    {
        None, Low, High
    }

    public interface IThreatMonitor
    {
        ThreatLevel CurrentThreatLevel { get; }

        event EventHandler ThreatLevelChanged;
    }
}
