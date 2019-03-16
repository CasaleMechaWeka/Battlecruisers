using BattleCruisers.Utils;

namespace BattleCruisers.AI.ThreatMonitors
{
    public class ThreatChangeSnapshot
    {
        public ThreatLevel ThreatLevel { get; }
        public float ChangeTimeSinceGameStartInS { get; }

        public ThreatChangeSnapshot(ThreatLevel threatLevel, float changeTimeSinceGameStartInS)
        {
            ThreatLevel = threatLevel;
            ChangeTimeSinceGameStartInS = changeTimeSinceGameStartInS;
        }

        public override bool Equals(object obj)
        {
            ThreatChangeSnapshot other = obj as ThreatChangeSnapshot;
            return
                other != null
                && ThreatLevel == other.ThreatLevel
                && ChangeTimeSinceGameStartInS == other.ChangeTimeSinceGameStartInS;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(ThreatLevel, ChangeTimeSinceGameStartInS);
        }
    }
}