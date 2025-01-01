using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters
{
    public class PvPAngleLimiterFactory : IPvPAngleLimiterFactory
    {
        public IAngleLimiter CreateDummyLimiter()
        {
            return new PvPDummyAngleLimiter();
        }

        public IAngleLimiter CreateFacingLimiter()
        {
            return new PvPFacingTurretAngleLimiter();
        }

        public IAngleLimiter CreateAntiAirLimiter()
        {
            return new PvPAntiAirAngleLimiter();
        }

        public IAngleLimiter CreateFighterLimiter()
        {
            return new PvPFighterAngleLimiter();
        }
        public IAngleLimiter CreateMissileFighterLimiter()
        {
            return new PvPMissileFighterAngleLimiter();
        }

        public IAngleLimiter CreateGravityAffectedLimiter()
        {
            return new PvPGravityAffectedTurretAngleLimiter();
        }

        public IAngleLimiter CreateCIWSLimiter()
        {
            return new PvPCIWSAngleLimiter();
        }
    }
}
