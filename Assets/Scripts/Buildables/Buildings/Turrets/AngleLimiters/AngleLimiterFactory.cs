namespace BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters
{
    public class AngleLimiterFactory
    {
        public IAngleLimiter CreateDummyLimiter()
        {
            return new DummyAngleLimiter();
        }

        public IAngleLimiter CreateFacingLimiter()
        {
            return new AngleLimiter(-30, 90);
        }

        public IAngleLimiter CreateAntiAirLimiter()
        {
            return new AngleLimiter(30, 150);
        }

        public IAngleLimiter CreateFighterLimiter()
        {
            return new AngleLimiter(-30, 30);
        }

        public IAngleLimiter CreateGravityAffectedLimiter()
        {
            return new AngleLimiter(-20, 85);
        }

        public IAngleLimiter CreateCIWSLimiter()
        {
            return new AngleLimiter(-30, 180);
        }
    }
}
