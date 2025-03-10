namespace BattleCruisers.AI.ThreatMonitors
{
	public class ThreatEvaluator : IThreatEvaluator
	{
        private readonly float _valueRequiredForHighThreatLevel;

        public ThreatEvaluator(float valueRequiredForHighThreatLevel)
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
