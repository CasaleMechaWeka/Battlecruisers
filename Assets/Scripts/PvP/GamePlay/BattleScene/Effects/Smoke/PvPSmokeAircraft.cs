using BattleCruisers.Effects.Smoke;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Smoke
{
    public class PvPSmokeAircraft : PvPSmoke
    {
        protected override SmokeStatistics GetStatsForStrength(PvPSmokeStrength strength)
        {
            switch (strength)
            {
                case PvPSmokeStrength.Weak:
                    return StaticSmokeStats.Aircraft.Weak;

                case PvPSmokeStrength.Normal:
                    return StaticSmokeStats.Aircraft.Normal;

                case PvPSmokeStrength.Strong:
                    return StaticSmokeStats.Aircraft.Strong;

                default:
                    return null;
            }
        }
    }
}