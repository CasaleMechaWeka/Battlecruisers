namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Smoke
{
    public class PvPSmokeShip : PvPSmoke
    {
        protected override PvPSmokeStatistics GetStatsForStrength(PvPSmokeStrength strength)
        {
            switch (strength)
            {
                case PvPSmokeStrength.Weak:
                    return PvPStaticSmokeStats.PvPShip.Weak;

                case PvPSmokeStrength.Normal:
                    return PvPStaticSmokeStats.PvPShip.Normal;

                case PvPSmokeStrength.Strong:
                    return PvPStaticSmokeStats.PvPShip.Strong;

                default:
                    return null;
            }
        }
    }
}