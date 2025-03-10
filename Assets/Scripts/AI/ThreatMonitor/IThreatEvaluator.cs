namespace BattleCruisers.AI.ThreatMonitors
{
    public interface IThreatEvaluator
	{
        ThreatLevel FindThreatLevel(float value);
	}
}
