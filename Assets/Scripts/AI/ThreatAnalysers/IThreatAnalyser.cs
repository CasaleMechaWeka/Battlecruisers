using System;

namespace BattleCruisers.AI.ThreatAnalysers
{
    public enum ThreatLevel
    {
        None, Low, High
    }

    public interface IThreatAnalyser
    {
        ThreatLevel CurrentThreatLevel { get; }

        event EventHandler ThreatLevelChanged;
    }
}
