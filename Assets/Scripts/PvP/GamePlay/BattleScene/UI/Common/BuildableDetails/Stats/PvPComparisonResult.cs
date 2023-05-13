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
        protected class PvPAlpha
        {
            public const float NEUTRAL = 1;
            public const float BETTER = 1;
            public const float WORSE = 0.5f;
        }

        protected class PvPHue
        {
            public static Color NEUTRAL = Color.white;
            public static Color BETTER = Color.white;
            public static Color WORSE = Color.black;
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
        public PvPNeutralResult() : base(PvPAlpha.NEUTRAL, PvPHue.NEUTRAL) { }
    }

    public class PvPBetterResult : PvPComparisonResult
    {
        public PvPBetterResult() : base(PvPAlpha.BETTER, PvPHue.BETTER) { }
    }

    public class PvPWorseResult : PvPComparisonResult
    {
        public PvPWorseResult() : base(PvPAlpha.WORSE, PvPHue.WORSE) { }
    }
}

