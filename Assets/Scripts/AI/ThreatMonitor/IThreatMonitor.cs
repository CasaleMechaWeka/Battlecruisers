using System;

namespace BattleCruisers.AI.ThreatMonitors
{
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
