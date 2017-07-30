namespace BattleCruisers.AI.ThreatAnalysers
{
    public interface IThreatEvaluator
	{
        ThreatLevel FindThreatLevel(int numOfDrones);
	}
}
