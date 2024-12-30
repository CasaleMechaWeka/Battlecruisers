using BattleCruisers.Effects.Smoke;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Smoke
{
    public class PvPSmokeBuilding : PvPSmoke
    {
        protected override SmokeStatistics GetStatsForStrength(PvPSmokeStrength strength)
        {
            switch (strength)
            {
                case PvPSmokeStrength.Weak:
                    return StaticSmokeStats.Building.Weak;

                case PvPSmokeStrength.Normal:
                    return StaticSmokeStats.Building.Normal;

                case PvPSmokeStrength.Strong:
                    return StaticSmokeStats.Building.Strong;

                default:
                    return null;
            }
        }
    }
}