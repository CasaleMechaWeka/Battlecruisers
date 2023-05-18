namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters
{
    public class PvPGravityAffectedTurretAngleLimiter : PvPAngleLimiter
    {
        public PvPGravityAffectedTurretAngleLimiter()
            : base(minAngle: 0, maxAngle: 85)
        {
        }
    }
}
