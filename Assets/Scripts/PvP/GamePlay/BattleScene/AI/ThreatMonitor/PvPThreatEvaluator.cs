namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors
{
    public class PvPThreatEvaluator : IPvPThreatEvaluator
    {
        private readonly float _valueRequiredForHighThreatLevel;

        public PvPThreatEvaluator(float valueRequiredForHighThreatLevel)
        {
            _valueRequiredForHighThreatLevel = valueRequiredForHighThreatLevel;
        }

        public PvPThreatLevel FindThreatLevel(float value)
        {
            if (value <= 0)
            {
                return PvPThreatLevel.None;
            }
            else if (value < _valueRequiredForHighThreatLevel)
            {
                return PvPThreatLevel.Low;
            }
            return PvPThreatLevel.High;
        }
    }
}
