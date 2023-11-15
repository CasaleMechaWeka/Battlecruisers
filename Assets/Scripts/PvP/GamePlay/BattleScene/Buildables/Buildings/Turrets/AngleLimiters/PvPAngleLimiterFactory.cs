namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters
{
    public class PvPAngleLimiterFactory : IPvPAngleLimiterFactory
    {
        public IPvPAngleLimiter CreateDummyLimiter()
        {
            return new PvPDummyAngleLimiter();
        }

        public IPvPAngleLimiter CreateFacingLimiter()
        {
            return new PvPFacingTurretAngleLimiter();
        }

        public IPvPAngleLimiter CreateAntiAirLimiter()
        {
            return new PvPAntiAirAngleLimiter();
        }

        public IPvPAngleLimiter CreateFighterLimiter()
        {
            return new PvPFighterAngleLimiter();
        }

        public IPvPAngleLimiter CreateGravityAffectedLimiter()
        {
            return new PvPGravityAffectedTurretAngleLimiter();
        }
    }
}
