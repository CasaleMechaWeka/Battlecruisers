namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public interface IComparisonResult
	{
        float RowAlpha { get; }
	}

	public abstract class ComparisonResult : IComparisonResult
	{
        protected class Alpha
        {
		    public static float NEUTRAL = 1;
            public static float BETTER = 1;
            public static float WORSE = 0.5f;
        }

        public float RowAlpha { get; }

        protected ComparisonResult(float rowAlpha)
		{
            RowAlpha = rowAlpha;
		}
	}

	public class NeutralResult : ComparisonResult
	{
		public NeutralResult() : base(Alpha.NEUTRAL) { }
	}

	public class BetterResult : ComparisonResult
	{
		public BetterResult() : base(Alpha.BETTER) { }
	}

	public class WorseResult : ComparisonResult
	{
		public WorseResult() : base(Alpha.WORSE) { }
	}
}

