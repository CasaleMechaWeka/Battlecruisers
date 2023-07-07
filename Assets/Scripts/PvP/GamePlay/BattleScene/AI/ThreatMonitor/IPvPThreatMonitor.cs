using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors
{
    // Do not change enum order, as the values are used for comparisons.
    // Ie:  myThreatLevel > ThreatLevel.Low
    public enum PvPThreatLevel
    {
        None, Low, High
    }

    public interface IPvPThreatMonitor
    {
        PvPThreatLevel CurrentThreatLevel { get; }

        event EventHandler ThreatLevelChanged;
    }
}
