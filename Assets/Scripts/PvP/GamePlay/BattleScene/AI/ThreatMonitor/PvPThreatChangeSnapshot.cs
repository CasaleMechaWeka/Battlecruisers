using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors
{
    public class PvPThreatChangeSnapshot
    {
        public ThreatLevel ThreatLevel { get; }
        public float ChangeTimeSinceGameStartInS { get; }

        public PvPThreatChangeSnapshot(ThreatLevel threatLevel, float changeTimeSinceGameStartInS)
        {
            ThreatLevel = threatLevel;
            ChangeTimeSinceGameStartInS = changeTimeSinceGameStartInS;
        }

        public override bool Equals(object obj)
        {
            PvPThreatChangeSnapshot other = obj as PvPThreatChangeSnapshot;
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