using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Damage
{
    public interface IPvPHealthThresholdMonitor
    {
        event EventHandler DroppedBelowThreshold;
        event EventHandler RoseAboveThreshold;
    }
}