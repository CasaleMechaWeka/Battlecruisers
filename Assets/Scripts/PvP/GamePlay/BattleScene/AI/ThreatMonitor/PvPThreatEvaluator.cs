using BattleCruisers.AI.ThreatMonitors;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors
{
    public class PvPThreatEvaluator : IPvPThreatEvaluator
    {
        private readonly float _valueRequiredForHighThreatLevel;

        public PvPThreatEvaluator(float valueRequiredForHighThreatLevel)
        {
            _valueRequiredForHighThreatLevel = valueRequiredForHighThreatLevel;
        }

        public ThreatLevel FindThreatLevel(float value)
        {
            if (value <= 0)
            {
                return ThreatLevel.None;
            }
            else if (value < _valueRequiredForHighThreatLevel)
            {
                return ThreatLevel.Low;
            }
            return ThreatLevel.High;
        }
    }
}
