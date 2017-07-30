namespace BattleCruisers.AI.ThreatMonitors
{
	public class ThreatEvaluator : IThreatEvaluator
	{
        private readonly int _numOfDronesRequiredForHighThreatLevel;

        public ThreatEvaluator(int numOfDronesRequiredForHighThreatLevel)
        {
            _numOfDronesRequiredForHighThreatLevel = numOfDronesRequiredForHighThreatLevel;
        }

		public ThreatLevel FindThreatLevel(int numOfDrones)
        {
			if (numOfDrones == 0)
			{
				return ThreatLevel.None;
			}
            else if (numOfDrones < _numOfDronesRequiredForHighThreatLevel)
			{
				return ThreatLevel.Low;
			}
			return ThreatLevel.High;
        }
	}
}
