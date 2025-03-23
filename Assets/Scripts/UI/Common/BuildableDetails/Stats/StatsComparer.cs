namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
	public static class HigherIsBetterComparer
	{
		public static ComparisonResult CompareStats(float stat1, float stat2)
		{
			if (stat1 == stat2)
				return new NeutralResult();
			else if (stat1 > stat2)
				return new BetterResult();

			return new WorseResult();
		}
	}

	public static class LowerIsBetterComparer
	{
		public static ComparisonResult CompareStats(float stat1, float stat2)
		{
			if (stat1 == stat2)
				return new NeutralResult();
			else if (stat1 < stat2)
				return new BetterResult();

			return new WorseResult();
		}
	}
}

