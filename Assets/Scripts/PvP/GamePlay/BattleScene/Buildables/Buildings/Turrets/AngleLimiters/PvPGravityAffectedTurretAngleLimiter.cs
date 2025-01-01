using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters
{
    public class PvPGravityAffectedTurretAngleLimiter : AngleLimiter
    {
        public PvPGravityAffectedTurretAngleLimiter()
            : base(minAngle: -20, maxAngle: 85)
        {
        }
    }
}
