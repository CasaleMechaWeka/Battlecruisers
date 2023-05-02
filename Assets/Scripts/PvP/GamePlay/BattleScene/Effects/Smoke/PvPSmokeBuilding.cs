namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Smoke
{
    public class PvPSmokeBuilding : PvPSmoke
    {
        protected override PvPSmokeStatistics GetStatsForStrength(PvPSmokeStrength strength)
        {
            switch (strength)
            {
                case PvPSmokeStrength.Weak:
                    return PvPStaticSmokeStats.PvPBuilding.Weak;

                case PvPSmokeStrength.Normal:
                    return PvPStaticSmokeStats.PvPBuilding.Normal;

                case PvPSmokeStrength.Strong:
                    return PvPStaticSmokeStats.PvPBuilding.Strong;

                default:
                    return null;
            }
        }
    }
}