using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters
{
    public class PvPAngleLimiterFactory : IAngleLimiterFactory
    {
        public IAngleLimiter CreateDummyLimiter()
        {
            return new DummyAngleLimiter();
        }

        public IAngleLimiter CreateFacingLimiter()
        {
            return new FacingTurretAngleLimiter();
        }

        public IAngleLimiter CreateAntiAirLimiter()
        {
            return new AntiAirAngleLimiter();
        }

        public IAngleLimiter CreateFighterLimiter()
        {
            return new FighterAngleLimiter();
        }
        public IAngleLimiter CreateMissileFighterLimiter()
        {
            return new MissileFighterAngleLimiter();
        }

        public IAngleLimiter CreateGravityAffectedLimiter()
        {
            return new GravityAffectedTurretAngleLimiter();
        }

        public IAngleLimiter CreateCIWSLimiter()
        {
            return new CIWSAngleLimiter();
        }
    }
}
