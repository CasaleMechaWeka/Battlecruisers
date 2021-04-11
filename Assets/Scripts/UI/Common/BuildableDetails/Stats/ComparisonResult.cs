using UnityEngine;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public interface IComparisonResult
	{
        float RowAlpha { get; }
		Color RowColour { get; }
	}

	public abstract class ComparisonResult : IComparisonResult
	{
        protected class Alpha
        {
		    public const float NEUTRAL = 1;
            public const float BETTER = 1;
            public const float WORSE = 0.5f;
        }

		protected class Hue
        {
			public static Color NEUTRAL = Color.white;
			public static Color BETTER = Color.white;
			public static Color WORSE = Color.black;
        }

        public float RowAlpha { get; }
        public Color RowColour { get; }

        protected ComparisonResult(float rowAlpha, Color rowColour)
		{
            RowAlpha = rowAlpha;
			RowColour = rowColour;
		}
	}

	public class NeutralResult : ComparisonResult
	{
		public NeutralResult() : base(Alpha.NEUTRAL, Hue.NEUTRAL) { }
	}

	public class BetterResult : ComparisonResult
	{
		public BetterResult() : base(Alpha.BETTER, Hue.BETTER) { }
	}

	public class WorseResult : ComparisonResult
	{
		public WorseResult() : base(Alpha.WORSE, Hue.WORSE) { }
	}
}

