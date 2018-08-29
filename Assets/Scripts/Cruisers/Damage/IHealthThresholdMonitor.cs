using System;

namespace BattleCruisers.Cruisers.Damage
{
    public interface IHealthThresholdMonitor
    {
        event EventHandler ThresholdReached;
    }
}