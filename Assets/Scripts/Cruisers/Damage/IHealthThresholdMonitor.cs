using System;

namespace BattleCruisers.Cruisers.Damage
{
    public interface IHealthThresholdMonitor
    {
        event EventHandler DroppedBelowThreshold;
        event EventHandler RoseAboveThreshold;
    }
}