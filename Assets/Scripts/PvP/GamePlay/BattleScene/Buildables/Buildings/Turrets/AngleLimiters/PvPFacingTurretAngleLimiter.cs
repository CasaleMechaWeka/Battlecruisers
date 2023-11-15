namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters
{
    public class PvPFacingTurretAngleLimiter : PvPAngleLimiter
    {
        public PvPFacingTurretAngleLimiter()
            : base(minAngle: -30, maxAngle: 90)
        {
        }
    }
}
