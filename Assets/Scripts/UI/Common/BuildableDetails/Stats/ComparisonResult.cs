using UnityEngine;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public interface IComparisonResult
	{
		Color BackgroundColor { get; }
		Color ForegroundColor { get; }
	}

	public abstract class ComparisonResult : IComparisonResult
	{
        protected class BackgroundColors
        {
		    public static Color NEUTRAL = Color.clear;
            public static Color BETTER = Color.white;
            public static Color WORSE = Color.clear;
        }

        protected class ForegroundColors
        {
            public static Color NEUTRAL = Color.white;
            public static Color BETTER = Color.black;
            public static Color WORSE = Color.white;
        }

		public Color BackgroundColor { get; private set; }
        public Color ForegroundColor { get; private set; }

        protected ComparisonResult(Color backgroundColor, Color textColor)
		{
			BackgroundColor = backgroundColor;
            ForegroundColor = textColor;
		}
	}

	public class NeutralResult : ComparisonResult
	{
		public NeutralResult() : base(BackgroundColors.NEUTRAL, ForegroundColors.NEUTRAL) { }
	}

	public class BetterResult : ComparisonResult
	{
		public BetterResult() : base(BackgroundColors.BETTER, ForegroundColors.BETTER) { }
	}

	public class WorseResult : ComparisonResult
	{
		public WorseResult() : base(BackgroundColors.WORSE, ForegroundColors.WORSE) { }
	}
}

