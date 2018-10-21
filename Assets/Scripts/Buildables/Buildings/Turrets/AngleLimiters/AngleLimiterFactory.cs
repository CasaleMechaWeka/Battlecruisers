namespace BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters
{
    public class AngleLimiterFactory : IAngleLimiterFactory{
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

        public IAngleLimiter CreateGravityAffectedLimiter()
        {
            return new GravityAffectedTurretAngleLimiter();
        }
    }
}
