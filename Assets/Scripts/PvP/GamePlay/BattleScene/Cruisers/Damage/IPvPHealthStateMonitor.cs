using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Damage
{
    public enum PvPHealthState
    {
        FullHealth,
        SlightlyDamaged,    // >= 2/3
        Damaged,            // >= 1/3
        SeverelyDamaged,    // < 1/3
        NoHealth
    }

    public interface IPvPHealthStateMonitor
    {
        PvPHealthState HealthState { get; }

        event EventHandler HealthStateChanged;
    }
}