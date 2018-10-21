namespace BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters
{
    public interface IAngleLimiterFactory
    {
        IAngleLimiter CreateDummyLimiter();
        IAngleLimiter CreateFacingLimiter();
        IAngleLimiter CreateAntiAirLimiter();
        IAngleLimiter CreateFighterLimiter();
        IAngleLimiter CreateGravityAffectedLimiter();
    }
}
