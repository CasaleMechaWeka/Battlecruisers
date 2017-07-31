namespace BattleCruisers.AI.ThreatMonitors
{
    public interface IThreatEvaluator
	{
        ThreatLevel FindThreatLevel(int numOfDrones);
	}
}
