using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters
{
    public class PvPAntiAirAngleLimiter : AngleLimiter
    {
        public PvPAntiAirAngleLimiter()
            : base(minAngle: 30, maxAngle: 150)
        {
        }
    }
}
