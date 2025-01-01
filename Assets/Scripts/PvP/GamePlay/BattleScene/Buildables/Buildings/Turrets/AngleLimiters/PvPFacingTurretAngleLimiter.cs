using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters
{
    public class PvPFacingTurretAngleLimiter : AngleLimiter
    {
        public PvPFacingTurretAngleLimiter()
            : base(minAngle: -30, maxAngle: 90)
        {
        }
    }
}
