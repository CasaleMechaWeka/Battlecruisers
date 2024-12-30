using BattleCruisers.Effects.Smoke;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Smoke
{
    public class PvPSmokeShip : PvPSmoke
    {
        protected override SmokeStatistics GetStatsForStrength(PvPSmokeStrength strength)
        {
            switch (strength)
            {
                case PvPSmokeStrength.Weak:
                    return StaticSmokeStats.Ship.Weak;

                case PvPSmokeStrength.Normal:
                    return StaticSmokeStats.Ship.Normal;

                case PvPSmokeStrength.Strong:
                    return StaticSmokeStats.Ship.Strong;

                default:
                    return null;
            }
        }
    }
}