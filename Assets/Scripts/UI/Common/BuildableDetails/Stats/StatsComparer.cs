namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public interface IStatsComparer
	{
		ComparisonResult CompareStats(float stat1, float stat2);
	}

	public class HigherIsBetterComparer : IStatsComparer
	{
		public ComparisonResult CompareStats(float stat1, float stat2)
		{
			if (stat1 == stat2)
			{
				return new NeutralResult();
			}
			else if (stat1 > stat2)
			{
				return new BetterResult();
			}
			return new WorseResult();
		}
	}

	public class LowerIsBetterComparer : IStatsComparer
	{
		public ComparisonResult CompareStats(float stat1, float stat2)
		{
			if (stat1 == stat2)
			{
				return new NeutralResult();
			}
			else if (stat1 < stat2)
			{
				return new BetterResult();
			}
			return new WorseResult();
		}
	}
}

