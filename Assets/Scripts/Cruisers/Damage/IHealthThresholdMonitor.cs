using System;

namespace BattleCruisers.Cruisers.Damage
{
    /// <summary>
    /// Keeps track of a damagable, and emits an event when that damagable's 
    /// health drops below a specified threshold.
    /// </summary>
    public interface IHealthThresholdMonitor
    {
        event EventHandler ThresholdReached;
    }
}