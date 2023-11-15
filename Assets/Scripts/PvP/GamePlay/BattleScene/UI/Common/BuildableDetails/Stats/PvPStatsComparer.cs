namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Stats
{
    public interface IPvPStatsComparer
    {
        PvPComparisonResult CompareStats(float stat1, float stat2);
    }

    public class PvPHigherIsBetterComparer : IPvPStatsComparer
    {
        public PvPComparisonResult CompareStats(float stat1, float stat2)
        {
            if (stat1 == stat2)
            {
                return new PvPNeutralResult();
            }
            else if (stat1 > stat2)
            {
                return new PvPBetterResult();
            }
            return new PvPWorseResult();
        }
    }

    public class PvPLowerIsBetterComparer : IPvPStatsComparer
    {
        public PvPComparisonResult CompareStats(float stat1, float stat2)
        {
            if (stat1 == stat2)
            {
                return new PvPNeutralResult();
            }
            else if (stat1 < stat2)
            {
                return new PvPBetterResult();
            }
            return new PvPWorseResult();
        }
    }
}

