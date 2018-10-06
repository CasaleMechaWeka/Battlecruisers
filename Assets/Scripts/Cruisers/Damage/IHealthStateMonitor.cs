using System;

namespace BattleCruisers.Cruisers.Damage
{
    public enum HealthState
    {
        FullHealth,
        SlightlyDamaged,    // >= 2/3
        Damaged,            // >= 1/3
        SeverelyDamaged     // < 1/3
    }

    public interface IHealthStateMonitor
    {
        HealthState HealthState { get; }

        event EventHandler HealthStateChanged;
    }
}