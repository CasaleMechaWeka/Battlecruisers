using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors
{
    public class PvPThreatChangeSnapshot
    {
        public PvPThreatLevel ThreatLevel { get; }
        public float ChangeTimeSinceGameStartInS { get; }

        public PvPThreatChangeSnapshot(PvPThreatLevel threatLevel, float changeTimeSinceGameStartInS)
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