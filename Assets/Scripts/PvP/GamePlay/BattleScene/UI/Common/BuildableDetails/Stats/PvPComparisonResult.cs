using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Stats
{
    public interface IPvPComparisonResult
    {
        float RowAlpha { get; }
        Color RowColour { get; }
    }

    public abstract class PvPComparisonResult : IPvPComparisonResult
    {
        protected class Alpha
        {
            public const float NEUTRAL = 1;
            public const float BETTER = 1;
            public const float WORSE = 1f;
        }

        protected class PvPHue
        {
            public static Color NEUTRAL = Color.white;
            public static Color BETTER = Color.white;
            public static Color WORSE = Color.white;
        }

        public float RowAlpha { get; }
        public Color RowColour { get; }

        protected PvPComparisonResult(float rowAlpha, Color rowColour)
        {
            RowAlpha = rowAlpha;
            RowColour = rowColour;
        }
    }

    public class PvPNeutralResult : PvPComparisonResult
    {
        public PvPNeutralResult() : base(Alpha.NEUTRAL, PvPHue.NEUTRAL) { }
    }

    public class PvPBetterResult : PvPComparisonResult
    {
        public PvPBetterResult() : base(Alpha.BETTER, PvPHue.BETTER) { }
    }

    public class PvPWorseResult : PvPComparisonResult
    {
        public PvPWorseResult() : base(Alpha.WORSE, PvPHue.WORSE) { }
    }
}

