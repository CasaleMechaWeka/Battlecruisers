namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Smoke
{
    public class PvPSmokeAircraft : PvPSmoke
    {
        protected override PvPSmokeStatistics GetStatsForStrength(PvPSmokeStrength strength)
        {
            switch (strength)
            {
                case PvPSmokeStrength.Weak:
                    return PvPStaticSmokeStats.PvPAircraft.Weak;

                case PvPSmokeStrength.Normal:
                    return PvPStaticSmokeStats.PvPAircraft.Normal;

                case PvPSmokeStrength.Strong:
                    return PvPStaticSmokeStats.PvPAircraft.Strong;

                default:
                    return null;
            }
        }
    }
}